using HymnalEntities.Hymnal;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                //Drop seed data
                _connection.DropTableAsync<Songbook>().Wait();
                _connection.DropTableAsync<Hymn>().Wait();
                _connection.DropTableAsync<Hymn_Lyric>().Wait();
                _connection.DropTableAsync<Hymn_Tag>().Wait();
                _connection.DropTableAsync<Lyric>().Wait();
                _connection.DropTableAsync<Tag>().Wait();
                // Create Tables
                _connection.CreateTableAsync<Songbook>().Wait();
                _connection.CreateTableAsync<Hymn>().Wait();
                _connection.CreateTableAsync<Hymn_Lyric>().Wait();
                _connection.CreateTableAsync<Hymn_Tag>().Wait();
                _connection.CreateTableAsync<Lyric>().Wait();
                _connection.CreateTableAsync<Tag>().Wait();
                // Seed test data
                PutSomething("Black Hymnal");
                PutSomething("Book 2");
                PutSomething("My Book");
            }
            catch (Exception ex)
            {
                //TODO: Logging.
            }
        }

        public Task<List<Songbook>> GetBooksWithSongs()
        {
            return _connection.Table<Songbook>().ToListAsync();
        }

        public void PutSomething(string title)
        {
            var sb = new Songbook()
            {
                Title = title
            };
            _connection.InsertOrReplaceAsync(sb).Wait(); // This sets the Id property
            for (int i = 0; i < 10 + title.Length; i++)
            {
                var hymn = new Hymn()
                {
                    Title = $"Hymn {i + 1}",
                    HymnNumber = i + 1,
                    SongbookId = sb.Id.Value
                };
                _connection.InsertOrReplaceAsync(hymn).Wait();
                string line = "This is a line\n";
                for (int j = 0; j < 5; j++)
                {
                    var lyric = new Lyric()
                    {
                        Order = j,
                        Verse = line + $"for hymn {hymn.HymnNumber}"
                    };
                    line = line + line;
                    _connection.InsertOrReplaceAsync(lyric).Wait();
                    _connection.InsertOrReplaceAsync(new Hymn_Lyric() { ParentId = hymn.Id.Value, ChildId = lyric.Id.Value });
                }
            }
        }

        public string GetSomething()
        {
            try
            {
                Songbook book = _connection.FindAsync<Songbook>(sb => sb.Id == 1).Result;
                if (book != null)
                {
                    return book.Title;
                }
            }
            catch (Exception ex)
            {
                // TODO: Logging
            }
            return "Not Found";
        }

        public int CountHymnsInSongbook(int? id)
        {
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

        public List<Lyric> GetLyricsForHymn(int? hymnId)
        {
            if (hymnId.GetValueOrDefault() == 0)
            {
                return new List<Lyric>();
            }
            var lyrics = _connection.Table<Hymn_Lyric>().Where(hl => hl.ParentId == hymnId.Value).ToListAsync().Result;
            return FetchLinkedEntities<Lyric, Hymn_Lyric>(hymnId);
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
