using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulRedux.Core.Models.Web
{
    public class Error
    {
        public string Type { get; set; }
        public string Reason { get; set; }
        public string StackTrace { get; set; }
        public bool IsPaywall { get; set; }
    }
}
