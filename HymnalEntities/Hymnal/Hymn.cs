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
        string Title { get; set; }
    }
}
