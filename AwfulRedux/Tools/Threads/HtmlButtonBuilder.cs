using System.Net;

namespace AwfulRedux.Tools.Threads
{
    public class HtmlButtonBuilder
    {
        /// <summary>
        /// Create HTML Submit button for Web Views.
        /// </summary>
        /// <param name="buttonName">Name of button.</param>
        /// <param name="buttonClick">Click handler to be applied to button.</param>
        /// <returns>HTML Submit Button String.</returns>
        public static string CreateSubmitButton(string buttonName, string buttonClick, string id, bool isSmall = true)
        {
            string webClass = isSmall ? "btn btn-sm" : "btn btn-previous";
            return WebUtility.HtmlDecode(
                       string.Format(
                           "<li><button class=\"{3}\" id=\"{2}\" type=\"submit\" onclick=\"{1}\";>{0}</button></li>",
                           buttonName, buttonClick, id, webClass));
        }
    }
}
