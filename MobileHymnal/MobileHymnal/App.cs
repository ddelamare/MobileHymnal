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
            Navigation = new NavigationPage(new Selector())
            {
                Icon = null,
                BarBackgroundColor = ConfigEngine.Current.NavigationBarColor
            };
            MasterDetail = new MasterDetailPage()
            {
                Master = new MenuDrawer() { Title = "Menu" },
                Detail = Navigation
            };
            MainPage = MasterDetail;
            Application.Current.Resources["configuration"] = ConfigEngine.Current;
            Navigation.ToolbarItems.Add(new ToolbarItem("Search", "", () =>
            {
                Hymnal.Navigation.Navigation.PushModalAsync(new SearchWindow(), true);
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

        public static NavigationPage Navigation { get; set; }
        public static MasterDetailPage MasterDetail { get; private set; }
    }
}
