using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TCMobile
{
    class API
    {
        public async void Commit(string cmi,string courseid)
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


                await App.Database.SaveItemAsync(courseRecord);
            }catch(Exception ex)
            {
                // do nothing some courses just commit way to fast because they are poorly designed.
            }



        }

        public  async void CommitToLMS(string cmi,string courseid)
        {
            cmi = System.Net.WebUtility.UrlEncode(cmi);
            string uri = Constants.SetCMI + "?studentid=" + Constants.StudentID + "&courseid=" + courseid + "&cmi=" + cmi;
            dynamic loginObj = await DataService.contactLMS(uri).ConfigureAwait(false);

            
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
            public string mode { get; set; }
            public string learner_id { get; set; }
            public string learner_name { get; set; }
            public string location { get; set; }
            public int progress_measure { get; set; }
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
            public List<object> objectives { get; set; }
            public StudentData student_data { get; set; }
            public LearnerPreference learner_preference { get; set; }
            public string assignments_submit { get; set; }
            public List<Interactions> interactions { get; set; }
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
