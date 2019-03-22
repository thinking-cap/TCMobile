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
            email = System.Net.WebUtility.UrlEncode(email);
            password = System.Net.WebUtility.UrlEncode(password);
            string uri = Constants.LoginURL + "?studentid=" + email + "&password=" + password + "&programid=" + Constants.ProgramID;
            dynamic loginObj = await DataService.contactLMS(uri).ConfigureAwait(false);

            return loginObj;
        }
    }


    public class ServiceResultOfString
    {
        public string Result { get; set; }
        public string Success { get; set; }
    }
    public class LogInObj
    {
        public string userId { get; set; }
        public string status { get; set; }
        public bool  login { get; set; }

        public string homedomain { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }

        public string blobLoc { get; set; }

        public string role { get; set; }
    }
}
