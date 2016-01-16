using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.Core.Models.PostIcons;

namespace AwfulRedux.Core.Models.Messages
{
    public class NewPrivateMessage
    {
        public PostIcon Icon { get; set; }

        public string Title { get; set; }

        public string Receiver { get; set; }

        public string Body { get; set; }
    }
}
