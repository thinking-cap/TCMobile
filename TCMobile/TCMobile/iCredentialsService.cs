using System;
using System.Collections.Generic;
using System.Text;

namespace TCMobile
{

    public interface ICredentialsService
    {
        string UserName { get; }

        string Password { get; }

        string UserID { get; }

        string FirstName { get; }

        string LastName { get; }

        String HomeDomain { get; }

        void SaveCredentials(string userName, string password, string userid,string firstname, string lastname ,string homedomain);

        void DeleteCredentials();

        bool DoCredentialsExist();
    }

}
