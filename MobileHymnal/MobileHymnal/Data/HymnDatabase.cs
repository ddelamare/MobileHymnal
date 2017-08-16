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
                // Create Tables
                _connection.CreateTableAsync<Songbook>().Wait();
                _connection.CreateTableAsync<Hymn>().Wait();
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
                _connection.InsertOrReplaceAsync(hymn);
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
    }
}
