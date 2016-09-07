using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.Database.DataSource;
using AwfulRedux.UI.Models.Threads;

namespace AwfulRedux.Database
{
    public class BookmarkDatabase
    {
        public static string DbLocation { get; set; }

        public BookmarkDatabase(string location)
        {
            DbLocation = location;
        }

        public async Task SetThreadNotified(Thread thread)
        {
            using (var bds = new Bookmarks(DbLocation))
            {
                thread.IsNotified = !thread.IsNotified;
                await bds.BookmarkThreads.Update(thread);
            }
        }

        public async Task RefreshBookmarkedThreads(List<Thread> updatedBookmarkList)
        {
            using (var bds = new Bookmarks(DbLocation))
            {
                var notifyThreads = await bds.BookmarkThreads.Items().Where(node => node.IsNotified).ToListAsync();
                var notifyThreadIds = notifyThreads.Select(thread => thread.ThreadId).ToList();

                await RemoveBookmarkThreads();
                var count = 0;
                foreach (Thread t in updatedBookmarkList)
                {
                    if (notifyThreadIds.Contains(t.ThreadId))
                    {
                        t.IsNotified = true;
                    }
                    t.Id = count;
                    count++;
                }

                await bds.BookmarkThreads.CreateAllWithChildren(updatedBookmarkList);
            }
        }

        public async Task RefreshBookmark(Thread updatedBookmark)
        {
            using (var bds = new Bookmarks(DbLocation))
            {
                await bds.BookmarkThreads.UpdateWithChildren(updatedBookmark);
            }
        }

        public async Task AddBookmark(Thread updatedBookmark)
        {
            using (var bds = new Bookmarks(DbLocation))
            {
                await bds.BookmarkThreads.CreateWithChildren(updatedBookmark);
            }
        }

        public async Task<Thread> GetBookmarkThreadAsync(long threadId)
        {
            using (var bds = new Bookmarks(DbLocation))
            {
                return await bds.BookmarkThreads.Items().Where(node => node.ThreadId == threadId).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> IsBookmark(long threadId)
        {
            using (var bds = new Bookmarks(DbLocation))
            {
                var result = await bds.BookmarkThreads.Items().Where(node => node.ThreadId == threadId).ToListAsync();
                return result.Count > 0;
            }
        }

        public async Task<List<Thread>> GetBookmarkedThreadsFromDb()
        {
            using (var bds = new Bookmarks(DbLocation))
            {
                return await bds.BookmarkThreads.Items().ToListAsync();
            }
        }

        public async Task AddBookmarkThreads(List<Thread> bookmarkedThreads)
        {
            using (var bds = new Bookmarks(DbLocation))
            {
                await bds.BookmarkThreads.CreateAllWithChildren(bookmarkedThreads);
            }
        }

        public async Task RemoveBookmarkThreads()
        {
            using (var bds = new Bookmarks(DbLocation))
            {
                var allBooksmarks = await bds.BookmarkThreads.Items().ToListAsync();
                await bds.BookmarkThreads.RemoveAll(allBooksmarks);
            }
        }
    }
}
