using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileHymnal.Data.Config
{
    public static class ConfigEngine
    {
        private static ISettings AppSettings => DependencyService.Get<IConfigStorage>().GetSettings();
        // TODO: Load themes
        public static Color NavigationBarColor {
            get
            {
                return FetchColor(defaultColor: new Color(.5,.5,.5));
            }
            set
            {
                SetColor(value);
            }
        }

        public static Color HymnTextColor
        {
            get
            {
                return FetchColor(defaultColor: Color.Black);
            }
            set
            {
                SetColor(value);
            }
        }

        public static Color HymnChorusColor
        {
            get
            {
                return FetchColor(defaultColor: Color.Gray);
            }
            set
            {
                SetColor(value);
            }
        }

        public static Color HymnBackgroundColor
        {
            get
            {
                return FetchColor(defaultColor:Color.White);
            }
            set
            {
                SetColor(value);
            }
        }

        // For Font size slider
        public static int FontSize
        {
            get => AppSettings.GetValueOrDefault(nameof(FontSize), 20);
            set => AppSettings.AddOrUpdateValue(nameof(FontSize), value);
        }

        #region Helpers
        private static Color FetchColor([CallerMemberName]string keyName = "", Color? defaultColor = null)
        {
            double r = 0, g = 0, b = 0;
            r = AppSettings.GetValueOrDefault(keyName + "_R", (double)defaultColor.GetValueOrDefault().R);
            g = AppSettings.GetValueOrDefault(keyName + "_G", (double)defaultColor.GetValueOrDefault().G);
            b = AppSettings.GetValueOrDefault(keyName + "_B", (double)defaultColor.GetValueOrDefault().B);
            return new Color(r, g, b);
        }

        private static void SetColor(Color color, [CallerMemberName]string keyName = "")
        {
            if (color == null)
            {
                //Reset 
                AppSettings.Remove(keyName + "_R");
                AppSettings.Remove(keyName + "_G");
                AppSettings.Remove(keyName + "_B");
            }
            AppSettings.AddOrUpdateValue(keyName + "_R", color.R);
            AppSettings.AddOrUpdateValue(keyName + "_G", color.G);
            AppSettings.AddOrUpdateValue(keyName + "_B", color.B);
        }
        #endregion
    }
}
