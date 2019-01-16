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

        void SaveCredentials(string userName, string password, string userid);

        void DeleteCredentials();

        bool DoCredentialsExist();
    }

}
