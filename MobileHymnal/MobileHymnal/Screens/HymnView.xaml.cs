using HymnalEntities.Hymnal;
using MobileHymnal.Data;
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
	public partial class HymnView : ContentPage
	{
        Hymn _hymn = null;
        private string textClassName = "textStyle";
		public HymnView (Hymn pageHymn)
		{
            InitializeComponent();
            _hymn = pageHymn;
            this.BindingContext = _hymn;
            var lyrics = Database.GetContext().GetLyricsForHymn(_hymn.Id);
            var verseNum = 1;
            foreach (Lyric l in lyrics.Result.OrderBy(l => l.Order))
            {
                if (l.IsChorus)
                {
                    lyricView.Children.Add(new Label()
                    {
                        Text = l.Verse,
                        Margin= new Thickness(20,10),
                        TextColor = Color.Gray,
                        Style = (Style)Resources[textClassName]
                    });
                }
                else
                {
                    lyricView.Children.Add(new Label()
                    {
                        Text = verseNum.ToString(),
                        FontAttributes = FontAttributes.Bold,
                        Style = (Style)Resources[textClassName]
                    });
                    verseNum++;
                    lyricView.Children.Add(new Label()
                    {
                        Text = l.Verse,
                        Style = (Style)Resources[textClassName]
                    });
                }
            }
        }
	}
}