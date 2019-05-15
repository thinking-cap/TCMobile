using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TCMobile.CustomControls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LearningPaths : ContentPage
    {
        public TCMobile.LPS lp;
        public LearningPaths()
        {
            InitializeComponent();
            

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadLearningPaths();
        }

        async void loadLearningPaths()
        {
            LP.Children.Clear();
            // show the spinner and turn it on 
            LPProgress.IsVisible = true;
            LPProgress.IsRunning = true;
            CredentialsService credentials = new CredentialsService();
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                lp = await Courses.GetLearningPaths(credentials.HomeDomain, credentials.UserID);
                var temp = lp.LearningPaths.OrderBy(o => o.title).ToList();
                buildLPS(temp);
            }
            else
            {
                buildLPSOffline();
            }
        }

        public async void buildLPSOffline()
        {
            Courses l = new Courses();
            List<Models.LPDBRecord> lps = await l.CheckForLPS();
            var temp = lps.OrderBy(o => o.LPTitle).ToList();
            if (lps != null)
            {
                foreach(Models.LPDBRecord lp in temp)
                {
                    Cards card = new Cards();
                    card.buildLPCard(lp.LPID, lp.LPTitle, lp.LPDescription, LP, DetailsClicked);
                }
            }
        }

        public void buildLPS(List<LPRecord> lp)
        {
            LPProgress.IsVisible = false;
            LPProgress.IsRunning = false;

            if (lp != null)
            {
                foreach (LPRecord l in lp)
                {
                    CreateLPRecord(l);
                }
            }
        }
        async void CreateLPRecord(LPRecord lp)
        {
            Models.LPDBRecord exists = await App.Database.GetLPByID(lp.id);
            if (exists == null)
            {
                Models.LPDBRecord lprecord = new Models.LPDBRecord();
                lprecord.LPID = lp.id;
                lprecord.LPTitle = lp.title;
                lprecord.LPDescription = lp.description;
                lprecord.LPCourses = "";
                await App.Database.SaveLPAsync(lprecord);
            }

            Cards card = new Cards();
            card.buildLPCard(lp.id, lp.title, lp.description, LP,DetailsClicked);
        }

        public void DetailsClicked(object sender, EventArgs e)
        {
            DownloadImageButton button = (DownloadImageButton)sender;
            string id = button.CourseID;
           
            Navigation.PushAsync(new LearningPath(id));
        }
    }
}