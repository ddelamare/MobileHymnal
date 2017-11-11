using HymnalEntities.Hymnal;
using MobileHymnal.Data;
using MobileHymnal.Data.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Runtime.CompilerServices;

namespace MobileHymnal.Screens
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HymnView : ContentPage
	{
        Hymn _hymn = null;
        private string textClassName = "textStyle";
		public HymnView (Hymn pageHymn)
        {
            InitializeComponent();
            _hymn = pageHymn;
            this.BindingContext = _hymn;
            BuildHymn();
        }

        public HymnView(int hymnId)
        {
            InitializeComponent();
            _hymn = Data.Database.GetContext().GetHymnById(hymnId);
            this.BindingContext = _hymn;
            BuildHymn();
        }

        private void BuildHymn()
        {
            var lyrics = Database.GetContext().GetLyricsForHymn(_hymn.Id);
            var verseNum = 1;
            foreach (Lyric l in lyrics.Result.OrderBy(l => l.Order))
            {
                if (l.IsChorus)
                {
                    var label = new Label()
                    {
                        Text = l.Verse,
                        Margin = new Thickness(20, 10),
                        TextColor = Color.Gray,
                    };
                    label.SetDynamicResource(VisualElement.StyleProperty, textClassName);
                    lyricView.Children.Add(label);
                }
                else
                {
                    lyricView.Children.Add(new Label()
                    {
                        Text = verseNum.ToString(),
                        FontAttributes = FontAttributes.Bold,
                        Style = (Style)(Style)Application.Current.Resources[textClassName]
                    });
                    verseNum++;
                    lyricView.Children.Add(new Label()
                    {
                        Text = l.Verse,
                        Style = (Style)(Style)Application.Current.Resources[textClassName]
                    });
                }
            }

            _hymn.Title = _hymn.GenerateTitle();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            Hymnal.Navigation.Title = "";
            base.OnDisappearing();
        }
    }
}