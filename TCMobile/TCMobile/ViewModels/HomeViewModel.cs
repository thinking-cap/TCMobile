using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace TCMobile.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
           Title = "My Courses";

            //OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

       // public ICommand OpenWebCommand { get; }
    }
}