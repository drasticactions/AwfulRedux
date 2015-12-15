using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AwfulRedux.Core.Models.Web;
using AwfulRedux.Core.Tools;

namespace AwfulRedux.Core.Managers
{
    public class AuthenticationManager
    {
        /// <summary>
        /// Authenticate a Something Awful user. This does not use the normal "WebManager" for handling the request
        /// because it requires we return the cookie container, so it can be used for actual authenticated requests.
        /// </summary>
        /// <param name="username">The Something Awful username.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="checkResult">Check the query string for login errors. Default is True.</param>
        /// <returns>An auth result object.</returns>
        public async Task<AuthResult> AuthenticateAsync(string username, string password, bool checkResult = true)
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = true,
                UseDefaultCredentials = false,
                CookieContainer = cookieContainer
            };
            using (var client = new HttpClient(handler))
            {
                var dic = new Dictionary<string, string>
                {
                    ["action"] = "login",
                    ["username"] = username,
                    ["password"] = password
                };
                client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.UtcNow;
                var header = new FormUrlEncodedContent(dic);
                var response = await client.PostAsync(new Uri(EndPoints.LoginUrl), header);
                var authResult = new AuthResult()
                {
                    IsSuccess = response.IsSuccessStatusCode && cookieContainer.Count >= 3,
                    AuthenticationCookie = cookieContainer
                };
                if (!checkResult)
                {
                    return authResult;
                }

                var queryString = Extensions.ParseQueryString(response.RequestMessage.RequestUri.Query);
                if (queryString["loginerror"] == null) return authResult;
                switch (queryString["loginerror"])
                {
                    case "1":
                        authResult.Error = "Failed to enter phrase from the security image.";
                        break;
                    case "2":
                        authResult.Error = "The password you entered is wrong. Remember passwords are case-sensitive! Be careful... too many wrong passwords and you will be locked out temporarily.";
                        break;
                    case "3":
                        authResult.Error = "The username you entered is wrong, maybe you should try 'idiot' instead? Watch out... too many failed login attempts and you will be locked out temporarily.";
                        break;
                    case "4":
                        authResult.Error =
                            "You've made too many failed login attempts. Your IP address is temporarily blocked.";
                        break;
                }

                return authResult;
            }
        }

    
    }
}
