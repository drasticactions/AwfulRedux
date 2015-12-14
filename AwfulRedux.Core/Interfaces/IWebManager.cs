using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.Core.Models.Web;

namespace AwfulRedux.Core.Interfaces
{
    public interface IWebManager
    {
        Task<Result> GetData(string uri);
        Task<Result> PostArchiveData(string uri, string data);
        Task<Result> PostData(string uri, FormUrlEncodedContent data);
        Task<CookieContainer> PostDataLogin(string url, string data);
        Task<Result> PostFormData(string uri, MultipartFormDataContent form);
    }
}
