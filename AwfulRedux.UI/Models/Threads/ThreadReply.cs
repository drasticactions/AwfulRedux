using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulRedux.UI.Models.Threads
{
    public class ThreadReply
    {
        public Thread Thread { get; set; }

        public int QuoteId { get; set; }
    }
}
