using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using TCMobile.Views;
using Xamarin.Forms;
using Microsoft.AppCenter.Crashes;

using Xamarin.Forms.Xaml;

namespace TCMobile
{
    class Courses
    {


        public static async Task<Catalogue> GetCatalogue(string domainid,string studentid)
        {
            try
            {
                string uri = Constants.StudentCatalogue + "?studentid=" + studentid + "&programid=" + domainid;
                dynamic results = await DataService.getDataFromService(uri).ConfigureAwait(false);


                if (results.courses != null)
                {
                    return results;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
                return null;
            }
            
           
        }

        public static async Task<LPS> GetLearningPaths(string domainid, string studentid)
        {
            try
            {
                string uri = Constants.LearningPaths + "?studentid=" + studentid + "&programid=" + domainid;
                dynamic results = await DataService.getLPs(uri).ConfigureAwait(false);
                return results;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<String>openCourse(string id, INavigation navigation)
        {
           //MainPage.Navigation.PushAsync(new ViewCourse(id));
           await navigation.PushAsync(new ViewCourse(id));

            return "test";
           
        }



        public async Task<List<Models.Record>>CheckForCourses()
        {

            List<Models.Record> courses = await App.Database.GetItemsAsync();
            
            return courses;
        }

        public async Task<List<Models.LPDBRecord>> CheckForLPS()
        {
            List<Models.LPDBRecord> lps = await App.Database.GetLPSAsync();

            return lps;

        }
    }
}
