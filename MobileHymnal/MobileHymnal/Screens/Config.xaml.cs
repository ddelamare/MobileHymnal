using MobileHymnal.Data.Config;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
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

        private async void importButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                FileData filedata = await CrossFilePicker.Current.PickFile();
                Data.Database.GetContext().ImportHymnal(filedata);
                await DisplayAlert("Import Success", "Import Complete.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Import Failed", "Importing the hymnal failed. Probably because it is not in the right format", "OK");
            }
        }
    }
}