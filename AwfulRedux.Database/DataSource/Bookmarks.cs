using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.Database.Tools;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.UI.Models.Users;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace AwfulRedux.Database.DataSource
{
    public class Bookmarks : IDisposable
    {
        protected SQLiteAsyncConnection Db { get; set; }

        public Repository<Thread> BookmarkThreads { get; set; }


        public Bookmarks(ISQLitePlatform platform, string dbPath)
        {
            var connectionFactory = new Func<SQLiteConnectionWithLock>(() => new SQLiteConnectionWithLock(platform, new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false)));
            Db = new SQLiteAsyncConnection(connectionFactory);

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
