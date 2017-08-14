using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileHymnal.Data
{
    public static class Database
    {
        static HymnDatabase _database = null;
        static string _fileName = "hymns.db3";

        public static HymnDatabase GetContext()
        { 
            if (_database != null)
                return _database;

            IFileHelper pathfinder = DependencyService.Get<IFileHelper>();
            var path = pathfinder.GetLocalFilePath(_fileName);
            _database = new HymnDatabase(path);
            return _database;
        }
    }
}
