using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;


namespace TCMobile
{
    class Courses
    {


        public static async Task<Catalogue> GetCatalogue(string domainid,string studentid)
        {
            string uri = Constants.StudentCatalogue + "?studentid=" + studentid + "&programid=" + domainid;
            dynamic results = await DataService.getDataFromService(uri).ConfigureAwait(false);
           
           
            
            if (results.courses != null)
            {
                System.Diagnostics.Debug.WriteLine("*********** START RESULTS *************", "Info");
                
               
               
                System.Diagnostics.Debug.WriteLine("*********** END RESULTS *************", "Info");
                return results;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("*********** ERROR RESULTS *************", "Info");
                return null;
            }
        }
    }
}
