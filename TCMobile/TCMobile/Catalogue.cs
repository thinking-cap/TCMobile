using System;
using System.Collections.Generic;
using System.Text;

namespace TCMobile
{


   

    public class Catalogue
    {
        //Because labels bind to these values, set them to an empty string to  
        //ensure that the label appears on all platforms by default.  
        public string id { get; set; }
        public List<Course>courses { get; set; }
    }

    public class Course
    {
        public string courseid { get; set; }
        public string domainid { get; set; }
        public string recordid { get; set; }
        public string duedate { get; set; }
        public string openIn { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public string enddate { get; set; }
        public string startdate { get; set; }
        public string status { get; set; }
        public string creditValue { get; set; }
        public string isOnline { get; set; }
        public string logo { get; set; }
        public string IsAccesscode { get; set; }
        public string canenter { get; set; }
        public string entermode { get; set; }

        public string version { get; set; }
    }
}
   
