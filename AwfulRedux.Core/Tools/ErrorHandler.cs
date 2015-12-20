using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.Core.Models.Web;
using Newtonsoft.Json;

namespace AwfulRedux.Core.Tools
{
    public class ErrorHandler
    {
        public static Result CreateErrorObject(Result result, string reason, string stacktrace, string type = "", bool isPaywall = false)
        {
            result.IsSuccess = false;
            result.Type = typeof (Error).ToString();
            var error = new Error()
            {
                Type = type,
                Reason = reason,
                StackTrace = stacktrace,
                IsPaywall = isPaywall
            };
            result.ResultJson = JsonConvert.SerializeObject(error);
            return result;
        }
    }
}
