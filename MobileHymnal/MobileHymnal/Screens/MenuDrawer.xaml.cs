using HymnalEntities.Hymnal;
using HymnalEntities.ViewModel;
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
    class HymnHistoryModel : BaseViewModel
    {
        public HymnHistoryModel()
        {
            History = new List<Hymn>();
        }

        private List<Hymn> _history;
        public List<Hymn> History
        {
            get { return _history; }
            set
            {
                _history = value;
                OnPropertyChanged();
            }
        }


    }
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuDrawer : ContentPage
	{
        private HymnHistoryModel _model;
        private static bool IsHandlerRegistered = false;
		public MenuDrawer ()
		{
            _model = new HymnHistoryModel();
            this.BindingContext = _model;
            _model.History = Data.Database.GetContext().GetHistory().Result;

            // Register Event Listener for Config
            if (!IsHandlerRegistered)
            {
                ConfigEngine.Current.PropertyChanged += HymnHistoryModel_PropertyChanged;
                IsHandlerRegistered = true;
            }
            InitializeComponent();
		}

        private void configButton_Clicked(object sender, EventArgs e)
        {
            Hymnal.MasterDetail.IsPresented = false;
            Hymnal.Navigation.PushAsync(new Config());
        }

        private void hymnHistory_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Hymnal.MasterDetail.IsPresented = false;
            Hymn selected = (Hymn)e.SelectedItem;
            ConfigEngine.Current.InsertHistory(selected?.Id ?? 0);
            Hymnal.Navigation.PushAsync(new HymnView(selected?.Id ?? 0));
        }

        protected override void OnAppearing()
        {
            _model.History = Data.Database.GetContext().GetHistory().Result;
            base.OnAppearing();
        }

        public void HymnHistoryModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            bool test = false;
            if (e.PropertyName == "InsertHistory" || test)
            {
                _model.History = Data.Database.GetContext().GetHistory().Result;
            }
        }
    }
}