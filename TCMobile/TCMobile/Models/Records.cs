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
        public string CMI { get; set; }
        public string Manifest { get; set; }
        public string Version { get; set; }
        public bool Done { get; set; }
        public string Deleted { get; set; }
    }
}
