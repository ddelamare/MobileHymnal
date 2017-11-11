using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HymnalEntities.Hymnal
{
    // This stores the content of the hymn. Each hymn can be in any number of songbooks, but with different reference numbers.
    public class Hymn : Entity
    {
        public Hymn() { }

        public int SongbookId { get; set; } //TODO?: Move to lookup table so the same hymn can be in several places?
        public int HymnNumber { get; set; }
        public string Title { get; set; }

        public string GenerateTitle()
        {
            return String.IsNullOrWhiteSpace(Title) ? $"Hymn #{HymnNumber}" : Title;
        }
    }
}
