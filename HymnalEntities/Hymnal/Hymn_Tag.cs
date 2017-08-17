using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HymnalEntities.Hymnal
{
    // A table to link tags to hymns
    public class Hymn_Tag : LinkTable
    {
        public new string ParentTable => "Hymn";
        public new string ChildTable => "Tag";
    }
}
