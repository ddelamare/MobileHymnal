using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using MobileHymnal.Data.Config;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using MobileHymnal.Droid.Data.Config;
using Xamarin.Forms;

[assembly: Dependency(typeof(ConfigStorage))]
namespace MobileHymnal.Droid.Data.Config
{
    public class ConfigStorage : IConfigStorage
    {
        public ISettings GetSettings()
        {
            return CrossSettings.Current;
        }
    }
}