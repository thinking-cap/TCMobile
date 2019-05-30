using System;
using System.Collections.Generic;
using System.Text;

namespace TCMobile
{
    class LP
    {
        
        public Models.statusObject lpStatus(List<Objective> objective)
        {
            int count = objective.Count;
            int completed = 0;
            int score = 0;
            foreach(var o in objective)
            {
                if (o.Status.ToLower() == "completed")
                    completed++;
                if(o.Score != "" && o.Score != "N/A")
                    score += Int32.Parse(o.Score);
            }
            Models.statusObject st = new Models.statusObject();
            st.completion = (count == completed) ? "completed" : "incomplete";
            st.percentage = (score / count) * 100;
            return st;
        }
    }
}
