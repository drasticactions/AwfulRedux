using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulRedux.Tools.Helper
{
    public class ToastNotificationArgs
    {
        public string type { get; set; }
        public long threadId { get; set; }

        public long forumId { get; set; }

        public int pageNumber { get; set; }

        public bool isThreadBookmark { get; set; }

        public bool openPrivateMessages { get; set; }

        public bool openBookmarks { get; set; }

        public bool openForum { get; set; }
    }
}
