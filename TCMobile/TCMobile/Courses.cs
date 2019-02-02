using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using TCMobile.Views;
using Xamarin.Forms;

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
            catch
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

       
    }
}
