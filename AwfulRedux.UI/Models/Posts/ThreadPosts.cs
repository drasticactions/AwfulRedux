using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.UI.Models.Threads;

namespace AwfulRedux.UI.Models.Posts
{
    public class ThreadPosts
    {
        public Thread ForumThread { get; set; }

        public List<Post> Posts { get; set; }
    }
}
