using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.Core.Models.Forums;
using AwfulRedux.Core.Models.PostIcons;

namespace AwfulRedux.Core.Models.Threads
{
    public class NewThread
    {
        public int ForumId { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public PostIcon PostIcon { get; set; }

        public string FormKey { get; set; }

        public string FormCookie { get; set; }

        public bool ParseUrl { get; set; }
    }
}
