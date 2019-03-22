using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Auth;

namespace TCMobile
{

    public class CredentialsService : ICredentialsService
    {
        public string UserName
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }

       public string UserID
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["UserID"] : null;
            }
        }

        public string HomeDomain
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? (account.Properties.ContainsKey("HomeDomain")) ? account.Properties["HomeDomain"] : null : null;
            }
        }

        public string Password
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["Password"] : null;
            }
        }

        public string FirstName
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["FirstName"] : null;
            }
        }

        public string BlobLoc
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["BlobLoc"] : null;
            }
        }

        public string LastName
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["LastName"] : null;
            }
        }

        public void SaveCredentials(string userName, string password, string userid,string firstname,string lastname, string homedomain, string blobloc)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("Password", password);
                account.Properties.Add("FirstName", firstname);
                account.Properties.Add("LastName", lastname);
                account.Properties.Add("UserID",userid);
                account.Properties.Add("HomeDomain", homedomain);
                account.Properties.Add("BlobLoc", blobloc);
                AccountStore.Create().Save(account, App.AppName);
            }

        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create().Delete(account, App.AppName);
            }
        }

        public bool DoCredentialsExist()
        {
            return AccountStore.Create().FindAccountsForService(App.AppName).Any() ? true : false;
        }
    }
}
