using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace TCMobile
{
    class userLogin
    {
        public static async Task<LogInObj> check(string email,string password)
        {
            string uri = Constants.Url + "?username=" + email + "&password=" + password + "&domainid=" + Constants.ProgramID;
            dynamic loginObj = await DataService.getDataFromService(uri).ConfigureAwait(false);

            return loginObj;
        }
    }

    public class LogInObj
    {
        public string userId { get; set; }
        public string status { get; set; }
        public bool  login { get; set; }

        public string role { get; set; }
    }
}
