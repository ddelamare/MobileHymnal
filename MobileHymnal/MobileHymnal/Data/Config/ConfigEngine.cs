using HymnalEntities.ViewModel;
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
    public class ConfigEngine : BaseViewModel
    {
        // Singleton instance
        private static readonly ConfigEngine instance = new ConfigEngine();
        public static ConfigEngine Current
        {
            get
            {
                return instance;
            }
        }
        private static ISettings AppSettings => CrossSettings.Current;
        // TODO: Load themes
        public Color NavigationBarColor {
            get
            {
                return FetchColor(defaultColor: new Color(.5,.5,.5));
            }
            set
            {
                SetColor(value);
                OnPropertyChanged();
            }
        }

        public Color HymnTextColor
        {
            get
            {
                return FetchColor(defaultColor: Color.Black);
            }
            set
            {
                SetColor(value);
                OnPropertyChanged();
            }
        }

        public Color HymnChorusColor
        {
            get
            {
                return FetchColor(defaultColor: Color.Gray);
            }
            set
            {
                SetColor(value);
                OnPropertyChanged();
            }
        }

        public Color HymnBackgroundColor
        {
            get
            {
                return FetchColor(defaultColor:Color.White);
            }
            set
            {
                SetColor(value);
                OnPropertyChanged();
            }
        }

        // For Font size slider
        public int FontSize
        {
            get => AppSettings.GetValueOrDefault(nameof(FontSize), 20);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(FontSize), value);
                OnPropertyChanged();
            }
        }

        #region Helpers
        private Color FetchColor([CallerMemberName]string keyName = "", Color? defaultColor = null)
        {
            double r = 0, g = 0, b = 0;
            r = AppSettings.GetValueOrDefault(keyName + "_R", (double)defaultColor.GetValueOrDefault().R);
            g = AppSettings.GetValueOrDefault(keyName + "_G", (double)defaultColor.GetValueOrDefault().G);
            b = AppSettings.GetValueOrDefault(keyName + "_B", (double)defaultColor.GetValueOrDefault().B);
            return new Color(r, g, b);
        }

        private void SetColor(Color color, [CallerMemberName]string keyName = "")
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
