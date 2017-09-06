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

            sampleLabel.Style = (Style)Application.Current.Resources["textStyle"];
        }

        private void closeButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void importButton_Clicked(object sender, EventArgs e)
        {
            waitSpinner.IsVisible = true;
            FileData filedata = await CrossFilePicker.Current.PickFile();
            try
            {
                var importSuccess = Data.Database.GetContext().ImportHymnal(filedata);
            }
            catch (Exception ex)
            {
                // TODO: Be graceful.
                throw;
            }
            waitSpinner.IsVisible = false;

        }

        protected override void OnDisappearing()
        {
            foreach (var page in Navigation.NavigationStack)
            {
                if (page is Selector)
                {
                    ((Selector)page).RefreshViewModel();
                }
                else if (page is HymnView)
                {
                    //TODO refresh bindings somehow
                }
            }
            base.OnDisappearing();
        }
    }
}