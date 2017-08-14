﻿using MobileHymnal.Data;
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

        // Global database instance
        static HymnDatabase database;

        public Hymnal()
        {
            // The root page of your application
            MainPage = new Selector();
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

        public static HymnDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new HymnDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return database;
            }
        }
    }
}
