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
                _connection.DropTableAsync<Songbook>().Wait();
                _connection.CreateTableAsync<Songbook>().Wait();
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
            var res = _connection.InsertOrReplaceAsync(sb).Result;
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
    }
}
