using System;
using System.Collections.Generic;
using System.Text;

namespace TCMobile
{
    public class ActivityMap
    {
        LearningPath LearningPath { get; set; }
        
    }
    public class LearningPath
    {
        StudentActivityMap StudentActivityMap { get; set; }
    }
    public class StudentActivityMap
    {
        Objective objective { get; set; }

    }

    public class Objective
    {
        string @id { get; set; }
        string Name { get; set; }
        string Status { get; set; }
        string Requited { get; set; }
        string PercentCompleted { get; set; }
        string Score { get; set; }
        Activities Activities { get; set; }

    }
    
    
    public class Activities
    {
        public List<Activity> activities { get; set; }
    }

    public class Activity
    {
        /*
            "@id": "ae43f059-c9dd-48b8-beb1-33b41bf9a426",
            "@mapID": "45f0c4a2-3e7f-49dc-9c2f-fba3888270c5",
            "@objectiveID": "f30545f7-6e5c-96a2-4ab8-9981c16c5dc5",
            "Required": "True",
            "CourseID": "ae43f059-c9dd-48b8-beb1-33b41bf9a426",
            "Name": "SCORMINATOR",
            "Type": "course",
            "AuthenticationType": "Managed By LMS",
            "Score": "0.99",
            "Status": "completed",
            "PercentCompleted": "100",
            "IsActive": "True"
        */
        string @id { get; set; }
        string @mapID { get; set; }
        string @objectiveID { get; set; }
        string Required { get; set; }
        string CourseID { get; set; }
        string Name { get; set; }
        string Type { get; set; }
        string AuthenticationType { get; set; }
        string Score { get; set; }
        string Status { get; set; }
        string PercentCompleted { get; set; }
        string IsActive { get; set; }
    }
}
