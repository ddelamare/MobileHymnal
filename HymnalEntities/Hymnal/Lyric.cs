using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HymnalEntities.Hymnal
{
    public class Lyric : Entity
    {
        public string Verse {get;set;}
        public int Order { get; set; } 
        public bool IsChorus { get; set; }
    }
}
