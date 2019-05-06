using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TCMobile.ViewModels;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NavBarView : ContentView
	{

        
        public NavBarView ()
		{
			InitializeComponent ();
           
		}

       //private void OpenMenu(object sender, EventArgs e)
       // {
       //     App app = Application.Current as App;
       //     MasterDetailPage md = (MasterDetailPage)app.MainPage;
       //     if (md.IsPresented)
       //     {
       //         md.IsPresented = false;
       //     }
       //     else
       //     {
       //         md.IsPresented = true;
       //     }
       // }
    }
}