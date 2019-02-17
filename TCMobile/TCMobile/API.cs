using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TCMobile
{
    class API
    {
        public async void Commit(string cmi,string courseid)
        {
            Models.Record courseRecord = await App.Database.GetCourseByID(courseid);
            courseRecord.CMI = cmi;
            await App.Database.SaveItemAsync(courseRecord);
        }

        public async Task<string>InitializeCourse(string courseid)
        {
            Models.Record courseRecord = await App.Database.GetCourseByID(courseid);
            return courseRecord.CMI;
        }

    }
}
