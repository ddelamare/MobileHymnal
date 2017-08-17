using HymnalEntities.Hymnal;
using MobileHymnal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MobileHymnal.Screens
{
    public class SelectorViewModel
    {
        public List<Songbook> SongbookList { get; set; }
        public Songbook SelectedSongbook { get; set; }
        public int MaxHymnNumber { get; set; }
    }

    public partial class Selector : ContentPage
    {
        SelectorViewModel _model;

        public Selector()
        {
            InitializeComponent();
            _model = BuildViewModel();
            SetHymnNumberMax();
            this.BindingContext = _model;
        }

        private SelectorViewModel BuildViewModel()
        {
            var vm = new SelectorViewModel();
            vm.SongbookList = Database.GetContext().GetBooksWithSongs().Result;
            vm.SelectedSongbook = vm.SongbookList.FirstOrDefault();
            return vm;
        }

        private void NumberPressed(object sender, EventArgs e)
        {
            hymnNumber.TextColor = Color.Black;
            // If either of these fail, it will return 0;
            int.TryParse(((Button)sender).Text, out int pressedNumber);
            int.TryParse(hymnNumber.Text, out int parsedNumber);
            // If text is numeric, append number. Otherwise clear and set.
            int newNumber = (parsedNumber * 10) + pressedNumber;
            if (newNumber > 0 && newNumber <= _model.MaxHymnNumber)
            {
                hymnNumber.Text = newNumber.ToString();
            }
        }

        private void ClearPressed(object sender, EventArgs e)
        {
            hymnNumber.TextColor = Color.Gray;
            hymnNumber.Text = "_ _ _ _";
        }

        async private void GoPressed(object sender, EventArgs e)
        {
            hymnNumber.TextColor = Color.Blue;
            int.TryParse(hymnNumber.Text, out int hymnNum);
            var hymn = Database.GetContext().GetHymnByNumber(_model.SelectedSongbook?.Id, hymnNum);
            if (hymn != null)
            {
                await Navigation.PushAsync(new HymnView(hymn));
            }
            else
            {
                await DisplayAlert("Hymn not found", $"Hymn number {hymnNum} was not found in selected hymnal.", "OK");
            }
        }

        private void SetHymnNumberMax()
        {
            // Todo: find number of hymns in current hymnal
            var hymnCount = Database.GetContext().CountHymnsInSongbook(_model.SelectedSongbook.Id);
            _model.MaxHymnNumber = hymnCount;
        }

        private void HymnalPickedChanged(object sender, EventArgs e)
        {
            SetHymnNumberMax();
        }
    }
}
