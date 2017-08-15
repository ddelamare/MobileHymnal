using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HymnalEntities.Hymnal
{
    public class Songbook : Entity
    {

        public string Title { get; set; }

        public override string ToString()
        {
            return $"{Title}";
        }
    }
}
