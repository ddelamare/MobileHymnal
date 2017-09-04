using MobileHymnal.Data;
using MobileHymnal.Data.Config;
using MobileHymnal.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileHymnal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Hymnal : Application
    {
        public Hymnal()
        {
            InitializeComponent();
            // Load 
            MainPage = new NavigationPage(new Selector()) {
                Icon = null,
                BarBackgroundColor = ConfigEngine.Current.NavigationBarColor
            };
            Application.Current.Resources["configuration"] = ConfigEngine.Current;
            MainPage.ToolbarItems.Add(new ToolbarItem("Config", "", () =>
            {
                ((NavigationPage)MainPage).PushAsync(new Config());
            }));
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
