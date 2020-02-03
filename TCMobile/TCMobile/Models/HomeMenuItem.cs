using System;
using System.Collections.Generic;
using System.Text;

namespace TCMobile.Models
{
    public enum MenuItemType
    {       
        LearningPaths,
        Catalogue,
        MyCourses,
        MyTranscripts,
        Settings,
        Logout

    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
