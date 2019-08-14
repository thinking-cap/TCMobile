using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TCMobile
{
    class LearningPath
    {
      public async Task<int> GetCompletion(string id)
        {

            Courses l = new Courses();
            Models.LPDBRecord lp = await l.GetActivityMap(id);
            int completionPercent = 0;
            if (lp != null && lp.LPMap != null && lp.LPMap != "")
            {
                JsonSerializerSettings ser = new JsonSerializerSettings();
                ser.DefaultValueHandling = DefaultValueHandling.Populate;
                StudentActivityMap lpMap = JsonConvert.DeserializeObject<StudentActivityMap>(lp.LPMap, ser);
                int objectiveCount = lpMap.Objective.Count;
                int completeCount = 0;
                
                foreach (Objective obj in lpMap.Objective)
                {                    
                    string status = obj.Status;
                    if (obj.Status.ToLower() == "completed")
                        completeCount++;
                }
                decimal p = (completeCount / objectiveCount) * 100;
                completionPercent = (int)Math.Round(p, 0, MidpointRounding.AwayFromZero);
            }
            return completionPercent;
        }
    }
}
