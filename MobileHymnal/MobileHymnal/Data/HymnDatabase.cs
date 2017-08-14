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
            _connection = new SQLiteAsyncConnection(connectionPath);
            _connection.DropTableAsync<Songbook>().Wait();
            _connection.CreateTableAsync<Songbook>().Wait();
        }

        public void PutSomething()
        {
            var sb = new Songbook()
            {
                Title = "Black Hymnal"
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
