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
            database.CreateTableAsync<Record>().Wait();
        }

        public Task<List<Models.Record>> GetItemsAsync()
        {
            return database.Table<Record>().ToListAsync();
        }

        public Task<List<Models.Record>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Models.Record>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<Models.Record> GetItemAsync(int id)
        {
            return database.Table<Models.Record>().Where(i => i.ID == id).FirstOrDefaultAsync();
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

        public Task<int> DeleteItemAsync(Models.Record item)
        {
            return database.DeleteAsync(item);
        }
    }
}
