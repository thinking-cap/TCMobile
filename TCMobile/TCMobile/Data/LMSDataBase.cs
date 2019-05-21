using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using TCMobile.Models;

namespace TCMobile.Data
{
    public class LMSDataBase
    {
        readonly SQLiteAsyncConnection database;

        public LMSDataBase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            /// build the course record table used to track courses
            database.CreateTableAsync<Record>().Wait();
            // build the lp record table
            // initially just to store the lp's for offline display.
            database.CreateTableAsync<LPDBRecord>().Wait();
            database.CreateTableAsync<LMSSettings>().Wait();
           // database.CreateTableAsync<StudentActivityMap>().Wait();
        }

        public Task<List<Record>> GetItemsAsync()
        {
            return database.Table<Record>().ToListAsync();
        }

        public Task<List<LPDBRecord>> GetLPSAsync()
        {
            return database.Table<LPDBRecord>().ToListAsync();
        }        

        public Task<Models.Record> GetCourseByID(string courseid)
        {
            return database.Table<Models.Record>().Where(i => i.CourseID == courseid).FirstOrDefaultAsync();
        }

        public Task<LPDBRecord> GetLPByID(string lpid)
        {
            return database.Table<LPDBRecord>().Where(i => i.LPID == lpid).FirstOrDefaultAsync();
        }

        public Task<Models.Record> GetItemAsync(int id)
        {
            return database.Table<Models.Record>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<List<LMSSettings>> GetSettings()
        {
            return database.Table<LMSSettings>().ToListAsync();
        }

        public Task<int> SaveSettings(LMSSettings item)
        {
            if(item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> SaveItemAsync(Models.Record item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int>SaveLpRecord(StudentActivityMap map)
        {
            return database.UpdateAsync(map);
        }

        public Task<int> SaveLPAsync(LPDBRecord lp)
        {
            if(lp.ID != 0)
            {
                return database.UpdateAsync(lp);
            }
            else
            {
                return database.InsertAsync(lp);
            }
        }

        public Task<int> DeleteItemAsync(Models.Record item)
        {
            return database.DeleteAsync(item);
        }
    }
}
