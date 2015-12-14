using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.UI.Models.Users;
using SQLite.Net.Interop;

namespace AwfulRedux.Database
{
    public class AuthenticatedUserDatabase
    {
        public static ISQLitePlatform Platform { get; set; }

        public static string DbLocation { get; set; }

        public AuthenticatedUserDatabase(ISQLitePlatform platform, string location)
        {
            Platform = platform;
            DbLocation = location;
        }

        public async Task<List<User>> GetAuthUsers()
        {
            using (var ds = new DataSource.MainForums(Platform, DbLocation))
            {
                return await ds.AuthenticatedUsers.Items.ToListAsync();
            }
        }

        public async Task<int> AddOrUpdateUser(User user)
        {
            using (var ds = new DataSource.MainForums(Platform, DbLocation))
            {
                var oldUser = await ds.AuthenticatedUsers.Items.Where(node => node.Id == user.Id).ToListAsync();
                if (oldUser.Any())
                {
                    return await ds.AuthenticatedUsers.Update(user);
                }
                return await ds.AuthenticatedUsers.Create(user);
            }
        }
    }
}
