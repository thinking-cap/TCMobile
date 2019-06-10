using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AppCenter.Crashes;

namespace TCMobile
{
    class API
    {
        public async Task<bool> Commit(string cmi,string courseid)
        {
            try
            {
                Models.Record courseRecord = await App.Database.GetCourseByID(courseid);
                courseRecord.CMI = cmi;
                Cmi data = null;
                data = JsonConvert.DeserializeObject<Cmi>(cmi);
                courseRecord.CompletionStatus = data.completion_status;
                courseRecord.SuccessStatus = data.success_status;
                courseRecord.Score = data.score.scaled;
                courseRecord.ProgressMeasure = data.progress_measure;


                await App.Database.SaveItemAsync(courseRecord);
                return true;
            }catch(Exception ex)
            {
                // do nothing some courses just commit way to fast because they are poorly designed.
                Crashes.TrackError(ex);
                return false;
            }



        }

        public async Task<bool> SyncCourse(string courseid)
        {
            Models.Record courseRecord = await App.Database.GetCourseByID(courseid);
            if (courseRecord != null)
            {
                string cmi = courseRecord.CMI;
                if (String.IsNullOrEmpty(cmi))
                    return false;// get out of dodge if there is nothing to sync
                bool send = await CommitToLMS(cmi, courseid);
                return send;
            }
            else { return false; }
        }

        public  async Task<bool> CommitToLMS(string cmi,string courseid)
        {
            try
            {
               // cmi = System.Net.WebUtility.UrlEncode(cmi);
               
                string uri = Constants.SetCMI + "?userPassword=" + App.CredentialsService.Password + "&userLogin=" + App.CredentialsService.UserName + "&courseID=" + courseid + "&scormObjectJson=" + cmi;
                dynamic loginObj = await DataService.commitToLMS(uri).ConfigureAwait(false);
                return true;
            }catch(Exception ex)
            {
                Crashes.TrackError(ex);
                return false;
            }

            
        }

        public async Task<dynamic>GetCMIFromLMS(string courseid)
        {
            string uri = Constants.GetCMI + "?userPassword=" + App.CredentialsService.Password + "&userLogin=" + App.CredentialsService.UserName + "&courseid=" + courseid;
            dynamic loginObj = await DataService.GetCMI(uri).ConfigureAwait(false);
            return loginObj;

        }

        public async Task<string>InitializeCourse(string courseid)
        {
            Models.Record courseRecord = await App.Database.GetCourseByID(courseid);
            if (courseRecord != null)
                return courseRecord.CMI;
            else
                return "";
        }


        

       

        public class StudentData
        {
            public List<string> _children { get; set; }
            public object __invalid_name__mastery_score  { get; set; }
            public object max_time_allowed { get; set; }
            public string time_limit_action { get; set; }
        }

        public class LearnerPreference
        {
            public List<string> _children { get; set; }
            public object audio { get; set; }
            public string language { get; set; }
            public object delivery_speed { get; set; }
            public string text { get; set; }
        }

        public class Cmi
        {
            public string _version { get; set; }
            public string course_id { get; set; }
            public string session_guid { set; get; }
            public string sco_id { get; set; }
            public string mode { get; set; }
            public string learner_id { get; set; }
            public string learner_name { get; set; }
            public string location { get; set; }
            public string progress_measure { get; set; }
            public Score score { get; set; }
            public string session_time { get; set; }
            public string success_status { get; set; }
            public string suspend_data { get; set; }
            public string completion_status { get; set; }
            public string credit { get; set; }
            public string entry { get; set; }
            public string exit { get; set; }
            public string total_time { get; set; }
            public string launch_data { get; set; }
            public string comments { get; set; }
            public string comments_from_lms { get; set; }
            public CommentsFromLearner comments_from_learner { get; set; }
            public List<Objectives> objectives { get; set; }
            public StudentData student_data { get; set; }
            public LearnerPreference learner_preference { get; set; }
            public string assignments_submit { get; set; }
            public List<Interactions> interactions { get; set; }
            public Interactions_Data interactions_data { get; set; }
            public Objectives_Data objectives_data { get; set; }

            public NavRequest nav_request { get; set; }

           
    }

        public class NavRequest
        {
            public string @continue {get;set;}
            public string choice { get; set; }
            public string previous { get; set; }            
    }

        public class Objectives
        {
            public string _children { get; set; }
            public string id { get; set; }
            public Score score { get; set; }
            public string description { get; set; }
            public string completion_status { get; set; }
            public string success_status { get; set; }
            public string progress_measure { get; set; }
        }

        public class Interactions_Data
        {
            //private string _children = "id,type,objectives,timestamp,correct_responses,weighting,learner_response,result,latency,description";
            public string _children { get; set; }
            public int _count { get; set; }
            public List<Interactions> interactions {get;set;}
        }

        public class Objectives_Data
        {
            //public string _children = "id,score,success_status,completion_status,description";
            public string _children { get; set; }
            public int _count { get; set; }
            public List<Objective> objectives { get; set; }
        }

        public class CommentsFromLearner
        {
            public string _children { get; set; }
            public List<object> comments { get; set; }
        }

        public class Score
        {
            public string raw { get; set; }
            public string max { get; set; }
            public string min { get; set; }
            public string scaled { get; set; }
        }

        public class Interactions
        {        
            public string id { get; set; }
            public List<object> objectives { get; set; }
            public string time { get; set; }
            public string type { get; set; }
            public string description { get; set; }
            public List<object> correct_responses { get; set; }
            public string result { get; set; }
            public string latency { get; set; }
            public string timestamp { get; set; }
        }

        public class Objective
        {
            public string _children { get; set; }
            public string id { get; set; }
            public Score score { get; set; }
            public string description { get; set; }
            public string completion_status { get; set; }
            public string success_status { get; set; }
            public string progress_measure { get; set; }
        }

        public class RootObject
        {
        public Cmi cmi { get; set; }
        }

    }
}
