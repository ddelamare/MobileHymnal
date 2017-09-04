using MobileHymnal.Data.Config;
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
    public partial class Config : ContentPage
    {
        public Config()
        {
            InitializeComponent();
            // Workaround for Xamarin bug on setting min val
                scaleSlider.Minimum = 8;
                scaleSlider.Value = ConfigEngine.Current.FontSize;
        }

        private void closeButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}