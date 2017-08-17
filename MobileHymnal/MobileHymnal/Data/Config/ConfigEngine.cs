using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileHymnal.Data.Config
{
    public static class ConfigEngine
    {
        // TODO: Load themes
        public static Color NavigationBarColor { get
            {
                return new Color(100, 100, 100);
            }
        }
    }
}
