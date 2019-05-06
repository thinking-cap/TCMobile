using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using TCMobile.Models;
using TCMobile.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TCMobile.ViewModels
{
    public class NavBarViewModel : BaseViewModel
    {
        ICommand menuCommand;
       
        public NavBarViewModel()
        {

            menuCommand = new Command(openMenu);
           
        }

        public ICommand MenuCommand
        {
            get { return menuCommand; }
        }

        
       

        
        void openMenu()
        {
            App app = Application.Current as App;
            MasterDetailPage md = (MasterDetailPage)app.MainPage;
            if (md.IsPresented)
            {
                md.IsPresented = false;
            }
            else
            {
                md.IsPresented = true;
            }
    }





    }
}
