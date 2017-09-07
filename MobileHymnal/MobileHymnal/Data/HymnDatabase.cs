using HymnalEntities.Hymnal;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Plugin.FilePicker.Abstractions;

namespace MobileHymnal.Data
{
    // This class should remain non-static and be managed by another class to maintain testability
    public class HymnDatabase
    {
        SQLiteAsyncConnection _connection;
        public HymnDatabase(string connectionPath)
        {
            try
            {
                _connection = new SQLiteAsyncConnection(connectionPath);
                
                InitDatabase(false);
            }
            catch (Exception ex)
            {
                //TODO: Logging.
            }
        }

        public void InitDatabase(bool seed)
        {

            if (seed)
            {
                //Drop seed data
                _connection.DropTableAsync<Songbook>().Wait();
                _connection.DropTableAsync<Hymn>().Wait();
                _connection.DropTableAsync<Hymn_Tag>().Wait();
                _connection.DropTableAsync<Lyric>().Wait();
                _connection.DropTableAsync<Tag>().Wait();
            }
            //Create Tables. TODO: Optimize so that it doesn't attempt to create every time.
            var initTasks = new List<Task>() {
            _connection.CreateTableAsync<Songbook>(),
            _connection.CreateTableAsync<Hymn>(),
            _connection.CreateTableAsync<Hymn_Tag>(),
            _connection.CreateTableAsync<Lyric>(),
            _connection.CreateTableAsync<Tag>()
            };

            Task.WaitAll(initTasks.ToArray());

            if (seed)
            {
                // Find all seed files
                var assembly = typeof(Database).GetTypeInfo().Assembly;
                var seedFiles = assembly.GetManifestResourceNames();
                foreach (var seedFile in seedFiles.Where(name => name.EndsWith(".seed")))
                {
                    Stream stream = assembly.GetManifestResourceStream(seedFile);
                    string rawJson = "";
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        rawJson = reader.ReadToEnd();
                    }
                    ImportHymnalFromJson(rawJson);
                }
            }
        }

        public bool ImportHymnal(FileData file)
        {
            var json = System.Text.Encoding.UTF8.GetString(file.DataArray, 0, file.DataArray.Length);
            return ImportHymnalFromJson(json);
        }

        private bool ImportHymnalFromJson(string rawJson)
        {
            var hymnDict = new Dictionary<Guid, Hymn>();
            var lyricDict = new Dictionary<Guid, List<Lyric>>();
            var songbook = JObject.Parse(rawJson);
            songbook.TryGetValue("Title", out JToken title);
            var sb = new Songbook()
            {
                Title = title?.Value<string>() ?? "You should really title this"
            };
            _connection.InsertOrReplaceAsync(sb).Wait(); // This sets the Id property

            var hymns = songbook["Hymns"];
            var tasks = new List<Task>();
            // Load it all into memory
            foreach (var h in hymns)
            {
                var insertGuid = Guid.NewGuid();
                var hm = new Hymn()
                {
                    SongbookId = sb.Id.GetValueOrDefault(),
                    HymnNumber = h["hymnNumber"].Value<int>()
                };
                hymnDict.Add(insertGuid, hm);

                List<Lyric> lyrics = new List<Lyric>();
                for (int i = 0; i < h["lyrics"].Count(); i++)
                {
                    var l = h["lyrics"][i];
                    var ly = new Lyric()
                    {
                        IsChorus = l["isChorus"].Value<bool>(),
                        Verse = l["text"].Value<string>(),
                        Order = i + 1
                    };
                    lyrics.Add(ly);
                }
                lyricDict.Add(insertGuid, lyrics);
            }
            _connection.InsertAllAsync(hymnDict.Values).Wait();
            var masterLyricsList = new List<Lyric>();
            foreach (var hymn in hymnDict)
            {
                var guid = hymn.Key;
                var hymnId = hymn.Value.Id.Value;
                var lyrics = lyricDict[guid];
                if (lyrics != null)
                {
                    foreach (var lyric in lyrics)
                    {
                        lyric.HymnId = hymnId;
                    }
                    masterLyricsList.AddRange(lyrics);
                }
            }
            _connection.InsertAllAsync(masterLyricsList);

            return true;
        }

        public Task<List<Songbook>> GetBooksWithSongs()
        {
            return _connection.Table<Songbook>().ToListAsync();
        }

        public int CountHymnsInSongbook(int? id)
        {
            // TODO: Change to max hymn number
            if (id.HasValue)
            {
                int bookId = id.GetValueOrDefault();
                return _connection.Table<Hymn>().Where(h => h.SongbookId == bookId).CountAsync().Result;
            }
            else
            {
                return 0;
            }
        }

        public Hymn GetHymnByNumber(int? songbookId, int? hymnNumber)
        {
            if (songbookId.HasValue && hymnNumber.HasValue)
            {
                return _connection.FindAsync<Hymn>(h => h.SongbookId == songbookId.Value && h.HymnNumber == hymnNumber.Value).Result;
            }
            else
            {
                return null;
            }
        }

        async public Task<List<Lyric>> GetLyricsForHymn(int? hymnId)
        {
            if (hymnId.GetValueOrDefault() == 0)
            {
                return new List<Lyric>();
            }
            return await _connection.Table<Lyric>().Where(l => l.HymnId == hymnId.Value).ToListAsync().ConfigureAwait(false);
        }

        // Attempt to auto join via link table. TODO: Add query caching
        public List<EntityType> FetchLinkedEntities<EntityType, LinkTableType>(int? entityId, bool isChildId = false) where LinkTableType : LinkTable where EntityType : Entity, new()
        {
            if (entityId.GetValueOrDefault() == 0)
            {
                return new List<EntityType>();
            }
            //Auto build join that returns EntityTypeList. if isChildId is true, then entityId should be the id of the child object.
            var parentOrChild = isChildId  ? "parent" : "child"; // What to return
            var childOrParent = isChildId ? "child" : "parent"; // What to match EntityId to
            var name = typeof(EntityType).Name;
            var linkTableName = typeof(LinkTableType).Name;
            // FIXME: Find a better way to do this.
            var parentTableName = linkTableName.Split('_')[0];
            var childTableName = linkTableName.Split('_')[1];

            var sql = $"SELECT {parentOrChild}.* FROM {parentTableName} as parent " + // SELECT {parentOrChild}.* FROM {parentTableName} as parent
                      $"INNER JOIN {linkTableName} as link on parent.Id = link.ParentId " + // INNER JOIN {linkTableName} as link on ...
                      $"INNER JOIN {childTableName} as child on link.ChildId = child.Id " + // INNER JOIN {childTableName} as child on ...
                      $"WHERE {childOrParent}.Id = {entityId}"; // Where {childOrParent}.Id = {entityId}

            try
            {  
                return _connection.QueryAsync<EntityType>(sql).Result;
            }
            catch (Exception ex)
            {
                return new List<EntityType>();
            }
        }
    }
}
