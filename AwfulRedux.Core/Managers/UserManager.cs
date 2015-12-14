using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.Core.Interfaces;
using AwfulRedux.Core.Models.Users;
using AwfulRedux.Core.Models.Web;
using AwfulRedux.Core.Tools;
using HtmlAgilityPack;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace AwfulRedux.Core.Managers
{
    public class UserManager
    {
        private readonly IWebManager _webManager;

        public UserManager(IWebManager webManager)
        {
            _webManager = webManager;
        }

        public async Task<Result> GetUserFromProfilePage(long userId, bool parseToJson = true)
        {
            string url = EndPoints.BaseUrl + string.Format(EndPoints.UserProfile, userId);
            var result = await _webManager.GetData(url);
            if (!result.IsSuccess || !parseToJson)
                return result;

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(result.ResultHtml);

                /*Get the user profile HTML from the user profile page,
                once we get it, get the nodes for each section of the page and parse them.*/

                HtmlNode profileNode = doc.DocumentNode.Descendants("td")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("info"));

                HtmlNode threadNode = doc.DocumentNode.Descendants("td")
                    .FirstOrDefault(node => node.GetAttributeValue("id", string.Empty).Contains("thread"));
                var userEntity = ParseFromUserProfile(profileNode, threadNode);
                result.ResultJson = JsonConvert.SerializeObject(userEntity);
            }
            catch (Exception ex)
            {
                ErrorHandler.CreateErrorObject(result, "Failed to parse user", ex.StackTrace, ex.GetType().FullName);
            }

            return result;
        }

        private User ParseFromUserProfile(HtmlNode profileNode, HtmlNode threadNode)
        {
            HtmlNode additionalNode =
                profileNode.Descendants("dl")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("additional"));
            Dictionary<string, string> additionalProfileAttributes = ParseAdditionalProfileAttributes(additionalNode);

            var user = new User
            {
                Username =
                    threadNode.Descendants("dt")
                        .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("author"))
                        .InnerText,
                AboutUser = string.Empty,
                DateJoined = DateTime.Parse(additionalProfileAttributes["Member Since"]),
                PostCount = int.Parse(additionalProfileAttributes["Post Count"]),
                PostRate = additionalProfileAttributes["Post Rate"]
            };

            foreach (HtmlNode aboutParagraph in profileNode.Descendants("p"))
            {
                user.AboutUser += WebUtility.HtmlDecode(aboutParagraph.InnerText.WithoutNewLines().Trim()) +
                                  Environment.NewLine + Environment.NewLine;
            }
            if (additionalProfileAttributes.ContainsKey("Seller Rating"))
            {
                user.SellerRating = additionalProfileAttributes["Seller Rating"];
            }
            if (additionalProfileAttributes.ContainsKey("Location"))
            {
                user.Location = additionalProfileAttributes["Location"];
            }
            return user;
        }

        private Dictionary<string, string> ParseAdditionalProfileAttributes(HtmlNode additionalNode)
        {
            IEnumerable<HtmlNode> dts = additionalNode.Descendants("dt");
            IEnumerable<HtmlNode> dds = additionalNode.Descendants("dd");
            Dictionary<string, string> result =
                dts.Zip(dds, (first, second) => new Tuple<string, string>(first.InnerText, second.InnerText))
                    .ToDictionary(k => k.Item1, v => v.Item2);
            // Clean up malformed HTML that results in the "last post" value being all screwy
            //string lastPostValue = result["Last Post"];
            //int removalStartIndex = lastPostValue.IndexOf('\n');
            //int lengthToRemove = lastPostValue.Length - removalStartIndex;
            //result["Last Post"] = lastPostValue.Remove(removalStartIndex, lengthToRemove);
            return result;
        }
    }
}
