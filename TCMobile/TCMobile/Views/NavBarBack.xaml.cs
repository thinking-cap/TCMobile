using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NavBarBack : ContentView
	{
		public NavBarBack ()
		{
			InitializeComponent ();
            BindingContext = new NavBarBackViewModel(Navigation);
        }
	}
}