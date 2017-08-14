using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using System.IO;
using Xamarin.Forms;
using MobileHymnal.Data;

using MobileHymnal.Droid.Data;

[assembly: Dependency(typeof(FileHelper))]
namespace MobileHymnal.Droid.Data
{
        public class FileHelper : IFileHelper
        {
            public string GetLocalFilePath(string filename)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                return Path.Combine(path, filename);
            }
        }   
}