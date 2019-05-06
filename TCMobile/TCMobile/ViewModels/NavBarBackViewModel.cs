using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;
using TCMobile.Views;
using System.Threading.Tasks;

namespace TCMobile.ViewModels
{
    public class NavBarBackViewModel : BaseViewModel
    {
        ICommand goBack;
        public INavigation Navigation { get; set; }

        public NavBarBackViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            goBack = new Command(async () => await goBackPage());
        }

        public ICommand GoBack
        {
            get { return goBack; }
        }
        public async Task goBackPage()
        {
            await Navigation.PushAsync(new LearningPaths());
        }
    }
}
