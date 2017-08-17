using MobileHymnal.Data;
using MobileHymnal.Data.Config;
using MobileHymnal.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MobileHymnal
{
    public class Hymnal : Application
    {
        public Hymnal()
        {
            // The root page of your application
            MainPage = new NavigationPage(new Selector()) {
                Icon = null,
                BarBackgroundColor = ConfigEngine.NavigationBarColor
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
