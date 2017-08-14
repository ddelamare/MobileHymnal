using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HymnalEntities.Hymnal
{
    public class Entity
    {
        [PrimaryKey, AutoIncrement]
        // This is nullable to force the database to insert a new row when the Id is null. 

        public int? Id { get; set; }

        public Entity()
        {
        }
    }
}
