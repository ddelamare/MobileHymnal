using HymnalEntities.Hymnal;
using HymnalEntities.ViewModel;
using MobileHymnal.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace MobileHymnal.Screens
{
    public class SelectorViewModel : BaseViewModel
    {
        public List<Songbook> SongbookList { get; set; }
        public Songbook SelectedSongbook { get; set; }
        public int MaxHymnNumber { get; set; }
        private string hymnLabel;
        public string HymnLabel
        {
            get
            {
                return hymnLabel;
            }
            set
            {
                hymnLabel = value;
                OnPropertyChanged();
            }

        }
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
            vm.HymnLabel = "_ _ _ _";
            return vm;
        }

        private void NumberPressed(object sender, EventArgs e)
        {
            // If either of these fail, it will return 0;
            int.TryParse(((Button)sender).Text, out int pressedNumber);
            int.TryParse(_model.HymnLabel, out int parsedNumber);
            // If text is numeric, append number. Otherwise clear and set.
            int newNumber = (parsedNumber * 10) + pressedNumber;
            if (newNumber > 0 && newNumber <= _model.MaxHymnNumber)
            {
                hymnNumber.TextColor = Color.Black;
                _model.HymnLabel = newNumber.ToString();
            }
        }

        private void ClearPressed(object sender, EventArgs e)
        {
            hymnNumber.TextColor = Color.Gray;
            _model.HymnLabel = "_ _ _ _";
        }

        async private void GoPressed(object sender, EventArgs e)
        {
            try
            {
                int.TryParse(_model.HymnLabel, out int hymnNum);
                var hymn = Database.GetContext().GetHymnByNumber(_model.SelectedSongbook?.Id, hymnNum);
                if (hymn != null)
                {
                    // TODO: Push to hymn view history
                    await Navigation.PushAsync(new HymnView(hymn));
                }
                else if (hymnNum > 0)
                {
                    await DisplayAlert("Hymn not found", $"Hymn number {hymnNum} was not found in selected hymnal.", "OK");
                }
            }
            catch (Exception ex)
            {

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

        protected override void OnAppearing()
        {
            ClearPressed(null, null);
            base.OnAppearing();
        }
    }
}
