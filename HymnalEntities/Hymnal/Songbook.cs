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


        private List<Hymn> _hymns;

        public void RefreshHymns()
        {
            _hymns = new List<Hymn>();
            //_hymns = GetHymnsForSongbook(Id);
        }

        public List<Hymn> Hymns
        {
            get
            {
                if (_hymns == null)
                {
                    RefreshHymns();
                }
                return _hymns;
            }
        }
    }
}
