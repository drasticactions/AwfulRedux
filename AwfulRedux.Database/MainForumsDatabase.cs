using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.UI.Models.Forums;
using SQLite.Net.Interop;

namespace AwfulRedux.Database
{
    public class MainForumsDatabase
    {
        public static ISQLitePlatform Platform { get; set; }

        public static string DbLocation { get; set; }

        public MainForumsDatabase(ISQLitePlatform platform, string location)
        {
            Platform = platform;
            DbLocation = location;
        }

        public async Task<List<Category>> GetMainForumsList()
        {
            using (var ds = new DataSource.MainForums(Platform, DbLocation))
            {
                var list = new List<Category>();
                var dbForumsCategories = await ds.ForumCategories.GetAllWithChildren();
                if (!dbForumsCategories.Any()) return list;
                var result = dbForumsCategories.OrderBy(node => node.Order);
                foreach (var forumCategoryEntity in result)
                {
                    var testForumList = new List<Forum>();
                    foreach (var forum in forumCategoryEntity.ForumList.Where(node => node.ParentForum == null))
                    {
                        testForumList.Add(forum);
                        var forum1 = forum;
                        testForumList.AddRange(forumCategoryEntity.ForumList.Where(node => node.ParentForum == forum1));
                    }
                    forumCategoryEntity.ForumList = testForumList;
                    list.Add(forumCategoryEntity);
                }
                return list;
            }
        }

        public async Task SaveMainForumsList(List<Category> forumGroupList)
        {
            using (var ds = new DataSource.MainForums(Platform, DbLocation))
            {
                await ds.ForumCategories.RemoveAll();
                await ds.Forums.RemoveAll();
                var count = 1;
                var forumCount = 1;
                foreach (var item in forumGroupList)
                {
                    foreach (var forumitem in item.ForumList)
                    {
                        forumitem.Id = count;
                        count++;
                    }
                    await ds.ForumCategories.CreateWithChildren(item);
                }
            }
        }

        public async Task<List<Forum>> GetFavoriteForumsAsync()
        {
            using (var ds = new DataSource.MainForums(Platform, DbLocation))
            {
                var list = new List<Forum>();
                var dbForumsCategories = await ds.Forums.GetAllWithChildren();
                if (!dbForumsCategories.Any()) return list;
                list.AddRange(dbForumsCategories.Where(node => node.IsBookmarks));
                return list;
            }
        }
    }
}
