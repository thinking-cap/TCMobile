using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Web;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;

namespace TCMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LearningPath : ContentPage
    {
        private Pages.LoadingPage _loadingPage;
        public LearningPath(string lpid)
        {
            App.CurrentLP = lpid;
            InitializeComponent();
           
        }

        protected override void OnAppearing()
        {
            _loadingPage = new Pages.LoadingPage("Loading");
            base.OnAppearing();
            loadCourses(App.CurrentLP);
        }
        private async void openPopup()
        {
            await PopupNavigation.Instance.PushAsync(_loadingPage);
        }

        private async void closePopup()
        {
            await PopupNavigation.Instance.RemovePageAsync(_loadingPage);
        }

        public TCMobile.Map lp;
        async void loadCourses(string lpid)
        {
            openPopup();
            LP.Children.Clear();
            CredentialsService credentials = new CredentialsService();
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet) { 
                lp = await Courses.GetLearningPath(credentials.HomeDomain, credentials.UserID, lpid);
                if(lp != null)
                {
                    bool x = await buildLPDetails(lp.StudentActivityMap,lpid);
                }
            }
            else
            {
                Courses l = new Courses();
                Models.LPDBRecord lp = await l.GetActivityMap(lpid);
                if(lp != null && lp.LPMap != null && lp.LPMap != "")
                {
                    JsonSerializerSettings ser = new JsonSerializerSettings();
                    ser.DefaultValueHandling = DefaultValueHandling.Populate;
                    StudentActivityMap lpMap = JsonConvert.DeserializeObject<StudentActivityMap>(lp.LPMap, ser);
                    bool x = await buildLPDetails(lpMap, lpid);
                }
            }

            closePopup();
        }

        async Task<bool>buildLPDetails(StudentActivityMap lp,string lpid)
        {
           // var x = await App.Database.SaveLpRecord(lp);
            Cards card = new Cards();
            bool x = await Courses.SaveMap(lpid, lp);
            foreach (Objective obj in lp.Objective)
            {
               await card.buildObjectiveCard(obj,LP,lpid);
            }
            return true;
        }
    }
}