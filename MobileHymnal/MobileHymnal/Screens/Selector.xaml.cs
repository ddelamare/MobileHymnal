using MobileHymnal.Data;
using System;
using Xamarin.Forms;

namespace MobileHymnal.Screens
{
    public partial class Selector : ContentPage
    {
        public Selector()
        {
            InitializeComponent();
        }

        void OnSliderValueChanged(object sender,
                                  ValueChangedEventArgs args)
        {
            valueLabel.Text = ((Slider)sender).Value.ToString("F3");
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            await DisplayAlert("Clicked!",
                "The button labeled '" + button.Text + "' has been clicked",
                "OK");
        }

        void OnPutDataClicked(object sender, EventArgs args)
        {
            var db = Database.GetContext();
            db.PutSomething();
        }

        async void OnGetDataClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            var db = Database.GetContext();
            
            await DisplayAlert("Clicked!",
                "The book titled' " + db.GetSomething() + "' has been found",
                "OK");
        }
    }
}
