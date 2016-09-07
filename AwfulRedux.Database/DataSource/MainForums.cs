using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.Database.Tools;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Users;
using SQLite;

namespace AwfulRedux.Database.DataSource
{
    /// <summary>
    /// Data for the main forums list
    /// </summary>
    public class MainForums : IDisposable
    {
        protected SQLiteAsyncConnection Db { get; set; }

        public Repository<Category> ForumCategories { get; set; }

        public Repository<Forum> Forums { get; set; }

        public Repository<User> AuthenticatedUsers { get; set; } 

        public MainForums(string dbPath)
        {
            Db = new SQLiteAsyncConnection(dbPath);

            ForumCategories = new Repository<Category>(Db);

            Forums = new Repository<Forum>(Db);

            AuthenticatedUsers = new Repository<User>(Db);
        }

        public void CreateDatabase()
        {
            var existingTables =
                Db.QueryAsync<sqlite_master>("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;")
                  .GetAwaiter()
                  .GetResult();

            if (existingTables.Any(x => x.name == "Forum") != true)
            {
                Db.CreateTableAsync<Forum>().GetAwaiter().GetResult();
            }

            if (existingTables.Any(x => x.name == "Category") != true)
            {
                Db.CreateTableAsync<Category>().GetAwaiter().GetResult();
            }

            if (existingTables.Any(x => x.name == "User") != true)
            {
                Db.CreateTableAsync<User>().GetAwaiter().GetResult();
            }
        }

        public void Dispose()
        {
            this.Db = null;
        }
    }
}
