using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.UI.Models.Users;

namespace AwfulRedux.Database
{
    public class AuthenticatedUserDatabase
    {
        public static string DbLocation { get; set; }

        public AuthenticatedUserDatabase(string location)
        {
            DbLocation = location;
        }

        public async Task<List<User>> GetAuthUsers()
        {
            using (var ds = new DataSource.MainForums(DbLocation))
            {
                return await ds.AuthenticatedUsers.Items().ToListAsync();
            }
        }

        public async Task<int> AddOrUpdateUser(User user)
        {
            using (var ds = new DataSource.MainForums(DbLocation))
            {
                var oldUser = await ds.AuthenticatedUsers.Items().Where(node => node.Id == user.Id).ToListAsync();
                if (oldUser.Any())
                {
                    return await ds.AuthenticatedUsers.Update(user);
                }
                return await ds.AuthenticatedUsers.Create(user);
            }
        }

        public async Task<int> RemoveUser(User user)
        {
            using (var ds = new DataSource.MainForums(DbLocation))
            {
                await ds.AuthenticatedUsers.Remove(user);
                return 1;
            }
        }
    }
}
