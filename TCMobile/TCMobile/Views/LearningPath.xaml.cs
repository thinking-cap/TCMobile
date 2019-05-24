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

namespace TCMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LearningPath : ContentPage
    {
        public LearningPath(string lpid)
        {
            InitializeComponent();
            loadCourses(lpid);
        }
        public TCMobile.Map lp;
        async void loadCourses(string lpid)
        {
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