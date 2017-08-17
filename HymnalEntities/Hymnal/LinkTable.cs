using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HymnalEntities.Hymnal
{
    public abstract class LinkTable
    {
        public int ParentId { get; set; }
        public int ChildId { get; set; }

        // To avoid relection at runtime, we can set these proerties at compile time
        public static string ParentTable => throw new NotImplementedException();
        public static string ChildTable => throw new NotImplementedException();
    }
}
