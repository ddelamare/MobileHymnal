using System;
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
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
            return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath , filename);
        }
    }
}