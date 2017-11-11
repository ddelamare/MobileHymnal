using HymnalEntities.Hymnal;
using HymnalEntities.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileHymnal.Screens
{
    class SearchViewModel : BaseViewModel
    {
        public SearchViewModel()
        {
            Results = new List<Lyric>();
        }

        public string SearchText { get; set; }
        private List<Lyric> _results;
        public List<Lyric> Results
        {
            get { return _results; }
            set {
                _results = value;
                OnPropertyChanged();
            }
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchWindow : ContentPage
    {
        private SearchViewModel _model;
        public SearchWindow()
        {
            _model = new SearchViewModel();
            this.BindingContext = _model;
            InitializeComponent();
        }

        private void searchField_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                if (_model.SearchText.Length > 0)
                {
                    _model.Results = Data.Database.GetContext().Search(_model.SearchText).Result;
                }
                else
                {
                    _model.Results = new List<Lyric>();
                }
                sw.Stop();
                System.Diagnostics.Debug.WriteLine($"Search time for {_model.SearchText}: {sw.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {

            }
        }

        private async void searchResults_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                await Hymnal.Navigation.PushAsync(new HymnView(((Lyric)e.SelectedItem).HymnId));
                await Hymnal.Navigation.Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {

            }
        }
    }
}