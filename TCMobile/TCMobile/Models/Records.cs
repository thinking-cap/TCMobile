using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace TCMobile.Models
{
    public class Record
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string CourseName { get; set; }
        public string CourseID { get; set; }
        public string CourseDescription { get; set; }
        public string CompletionStatus { get; set; }
        public string SuccessStatus { get; set; }
        public string Score { get; set; }
        public string ProgressMeasure { get; set; }
        public string CMI { get; set; }
        public string Manifest { get; set; }
        public string Version { get; set; }
        public bool Done { get; set; }
        public string Deleted { get; set; }
        public bool Downloaded { get; set; }
        public string DueDate { get; set; }
        public string LP { get; set; }
        public string Objective { get; set; }
        public string PercentComplete { get; set; }
        public bool Synced { get; set; }
    }

    public class LPDBRecord
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string LPTitle { get; set; }
        public string LPDescription { get; set; }
        public string LPID { get; set; }
        public string LPMap { get; set; }

    }

    public class LMSSettings
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string PrimaryBG { get; set; }
        public string SecondaryBG { get; set; }
        public bool WiFiOnly { get; set; }
    }
}
