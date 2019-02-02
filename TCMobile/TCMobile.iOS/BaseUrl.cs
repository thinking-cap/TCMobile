using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using TCMobile.iOS;

[assembly: Dependency(typeof(BaseUrl))]

namespace TCMobile.iOS
{
    class BaseUrl : iBaseURL
    {
        public string Get()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //return "file:/" + System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) +
            //"Courses/2d7d0a7d-145a-41d0-9abf-685a2b5dfc3c/Online_Placement_Test_no_timer_pack/YKZOP4NACH3EPJNTG6M4T2BQDI/Unit_4_5/995/";
        }
    }
}