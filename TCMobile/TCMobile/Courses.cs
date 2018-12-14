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
    }
}
