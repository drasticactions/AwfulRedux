using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AwfulRedux.Core.Models.Web
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }

        public string Error { get; set; }

        public CookieContainer AuthenticationCookie { get; set; }
    }
}
