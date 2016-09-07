using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.Database.Tools;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.UI.Models.Users;
using SQLite;

namespace AwfulRedux.Database.DataSource
{
    public class Bookmarks : IDisposable
    {
        protected SQLiteAsyncConnection Db { get; set; }

        public Repository<Thread> BookmarkThreads { get; set; }


        public Bookmarks(string dbPath)
        {
            Db = new SQLiteAsyncConnection(dbPath);
            BookmarkThreads = new Repository<Thread>(Db);
        }

        public void CreateDatabase()
        {
            var existingTables =
                Db.QueryAsync<sqlite_master>("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;")
                  .GetAwaiter()
                  .GetResult();

            if (existingTables.Any(x => x.name == "Thread") != true)
            {
                Db.CreateTableAsync<Thread>().GetAwaiter().GetResult();
            }
        }

        public void Dispose()
        {
            this.Db = null;
        }
    }
}
