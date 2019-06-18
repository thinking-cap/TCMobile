using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;

namespace TCMobile.Pages
{
    
    public partial class LoadingPage : PopupPage
    {
       

        public LoadingPage(string msg)
        {
           
            InitializeComponent();
            Msg.Text = msg;
        }
    }
}