using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            lp = await Courses.GetLearningPath(credentials.HomeDomain, credentials.UserID, lpid);
            if(lp != null)
            {
                bool x = await buidLPDetails(lp.StudentActivityMap);
            }

        }

        async Task<bool>buidLPDetails(StudentActivityMap lp)
        {
            Cards card = new Cards();
            foreach (Objective obj in lp.Objective)
            {
               await card.buildObjectiveCard(obj,LP);
            }
            return true;
        }
    }
}