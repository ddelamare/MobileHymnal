using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileHymnal.Screens
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuDrawer : ContentPage
	{
		public MenuDrawer ()
		{
			InitializeComponent ();
		}

        private void configButton_Clicked(object sender, EventArgs e)
        {
            Hymnal.MasterDetail.IsPresented = false;
            Hymnal.Navigation.PushAsync(new Config());
        }
    }
}