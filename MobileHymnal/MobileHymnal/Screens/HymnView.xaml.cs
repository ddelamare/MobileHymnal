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
		public HymnView (Hymn pageHymn)
		{
            InitializeComponent();
            _hymn = pageHymn;
            this.BindingContext = _hymn;
            var lyrics = Database.GetContext().GetLyricsForHymn(_hymn.Id);
            foreach (Lyric l in lyrics)
            {
                lyricView.Children.Add(new Label()
                {
                    Text = l.Verse
                });
            }
        }
	}
}