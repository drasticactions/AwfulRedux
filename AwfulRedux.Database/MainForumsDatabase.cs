using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.UI.Models.Forums;
namespace AwfulRedux.Database
{
    public class MainForumsDatabase
    {
        public static string DbLocation { get; set; }

        public MainForumsDatabase(string location)
        {
            DbLocation = location;
        }

        public async Task<List<Category>> GetMainForumsList()
        {
            using (var ds = new DataSource.MainForums(DbLocation))
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
            using (var ds = new DataSource.MainForums(DbLocation))
            {
                var categories = await ds.ForumCategories.Items().ToListAsync();
                var forums = await ds.Forums.Items().ToListAsync();
                await ds.ForumCategories.RemoveAll(categories);
                await ds.Forums.RemoveAll(forums);

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
            using (var ds = new DataSource.MainForums(DbLocation))
            {
                var list = new List<Forum>();
                var dbForumsCategories = await ds.Forums.GetAllWithChildren();
                if (!dbForumsCategories.Any()) return list;
                list.AddRange(dbForumsCategories.Where(node => node.IsBookmarks));
                return list;
            }
        }

        public async Task UpdateForum(Forum forum)
        {
            using (var ds = new DataSource.MainForums(DbLocation))
            {
                await ds.Forums.Update(forum);
            }
        }
    }
}
