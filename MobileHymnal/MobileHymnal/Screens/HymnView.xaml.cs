using HymnalEntities.Hymnal;
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
            try
            {
                InitializeComponent();
            }
            catch (Exception ec)
            {

            }
            _hymn = pageHymn;
            this.BindingContext = _hymn;
		}
	}
}