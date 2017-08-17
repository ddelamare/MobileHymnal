using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HymnalEntities.Hymnal
{
    public class Hymn_Lyric : LinkTable
    {
        public new string ParentTable => "Hymn";
        public new string ChildTable => "Lyric";
    }
}
