using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AwfulRedux.Core.Interfaces;
using AwfulRedux.Core.Models.Polls;
using AwfulRedux.Core.Models.Posts;
using AwfulRedux.Core.Models.Threads;
using AwfulRedux.Core.Models.Web;
using AwfulRedux.Core.Tools;
using HtmlAgilityPack;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace AwfulRedux.Core.Managers
{
    public class PostManager
    {
        private readonly IWebManager _webManager;

        public PostManager(IWebManager webManager)
        {
            _webManager = webManager;
        }

        public async Task<HtmlDocument> GetThreadInfo(Thread forumThread, string url)
        {
            var result = await _webManager.GetData(url);
           
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result.ResultHtml);
            try
            {
                ParseFromThread(forumThread, doc);
            }
            catch (Exception exception)
            {
                throw new Exception("Error parsing thread", exception);
            }

            try
            {
                string responseUri = result.AbsoluteUri;
                string[] test = responseUri.Split('#');
                if (test.Length > 1 && test[1].Contains("pti"))
                {
                    forumThread.ScrollToPost = Int32.Parse(Regex.Match(responseUri.Split('#')[1], @"\d+").Value) - 1;
                    forumThread.ScrollToPostString = string.Concat("#", responseUri.Split('#')[1]);
                }

                var query = Extensions.ParseQueryString(new Uri(url).Query);

                if (query["pagenumber"] != null)
                {
                    forumThread.CurrentPage = Convert.ToInt32(query["pagenumber"]);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error parsing thread", exception);
            }

            return doc;
        }

        public async Task<Result> GetThreadPostsAsync(string location, int currentPage, bool hasBeenViewed = false)
        {
            string url = location;

            if (currentPage > 0)
            {
                url = location + string.Format(EndPoints.PageNumber, currentPage);
            }
            else if (hasBeenViewed)
            {
                url = location + EndPoints.GotoNewPost;
            }

            var forumThreadPosts = new List<Post>();

            //var threadManager = new ThreadManager();
            //var doc = await GetThreadInfo(forumThread, url);

            var result = await _webManager.GetData(url);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result.ResultHtml);
            try
            {

                try
                {
  //                  HtmlNode pollNode =
  //doc.DocumentNode.Descendants("form")
  //    .FirstOrDefault(node => node.GetAttributeValue("action", string.Empty).Equals("poll.php"));

  //                  if (pollNode != null)
  //                  {
  //                      forumThread.Poll = ParsePoll(doc);
  //                  }

                }
                catch (Exception)
                {

                    // Failed to get poll. Ignore and continue...
                }

                HtmlNode threadNode =
                   doc.DocumentNode.Descendants("div")
                       .FirstOrDefault(node => node.GetAttributeValue("id", string.Empty).Contains("thread"));

                foreach (
                   HtmlNode postNode in
                       threadNode.Descendants("table")
                           .Where(node => node.GetAttributeValue("class", string.Empty).Contains("post") && !string.IsNullOrEmpty(node.GetAttributeValue("data-idx", string.Empty))))
                {
                    var post = new Post();
                    ParsePost(post, postNode);
                    var postBodyNode =
                        postNode.Descendants("td")
                            .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("postbody"));
                    var query =
                        postBodyNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("class", string.Empty) == "bbc-block");
                    foreach (var item in query.ToList())
                    {
                        var newHeadNode = HtmlNode.CreateNode(item.InnerHtml);
                        item.ParentNode.ReplaceChild(newHeadNode.ParentNode, item);
                    }

                    var h4Query = postBodyNode.Descendants("h4");
                    foreach (var h4 in h4Query.ToList())
                    {
                        var newHeadNode = HtmlNode.CreateNode($"<h4>{h4.InnerText}</h4>");
                        h4.ParentNode.ReplaceChild(newHeadNode.ParentNode, h4);
                    }

                    forumThreadPosts.Add(post);
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse thread posts {ex.Message}");
            }
            result.ResultJson = JsonConvert.SerializeObject(forumThreadPosts);
            return result;
        }

        public async Task<Result> GetPostAsync(int postId, bool parseToJson = true)
        {
            try
            {
                var url = string.Format(EndPoints.ShowPost, postId);
                var result = await _webManager.GetData(url);
                if (!result.IsSuccess) return result;

                if (!parseToJson)
                    return result;

                var doc = new HtmlDocument();
                doc.LoadHtml(result.ResultHtml);
                var threadNode =
                    doc.DocumentNode.Descendants("div")
                        .FirstOrDefault(node => node.GetAttributeValue("id", string.Empty).Contains("thread"));
                var postNode =
                    threadNode.Descendants("table")
                        .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("post"));
                var post = new Post();

                ParsePost(post, postNode);

                result.ResultJson = JsonConvert.SerializeObject(post);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting post", ex);
            }
        }

        public PollGroup ParsePoll(HtmlDocument pollNode)
        {
            var pollid =
               Convert.ToInt32(pollNode.DocumentNode.Descendants("input")
                    .First(node => node.GetAttributeValue("name", string.Empty).Equals("pollid")).GetAttributeValue("value", string.Empty));

            var tableRows = pollNode.DocumentNode.Descendants("table").First(node => node.GetAttributeValue("id", string.Empty).Equals("main_full")).Descendants("tr").ToArray();
            var pollEntities = new List<PollItem>(tableRows.Length - 2);
            for (var i = 1; i <= tableRows.Length - 1; i++)
            {
                pollEntities.Add(new PollItem
                {
                    Id = i,
                    Title = tableRows[i].InnerText.WithoutNewLines()
                });
            }

            return new PollGroup()
            {
                Title = WebUtility.HtmlDecode(tableRows[0].InnerText),
                Id = pollid,
                PollList = pollEntities
            };
        }

        public void ParsePost(Post post, HtmlNode postNode, bool isSimple = false)
        {
            post.User = UserManager.ParseNewUserFromPost(postNode);

            HtmlNode postDateNode =
                postNode.Descendants()
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("postdate"));
            string postDateString = postDateNode == null ? string.Empty : postDateNode.InnerText;
            if (postDateString != null)
            {
                post.PostDate = postDateString.WithoutNewLines().Trim();
            }

            post.PostIndex = ParseInt(postNode.GetAttributeValue("data-idx", string.Empty));

            var postId = postNode.GetAttributeValue("id", string.Empty);
            if (!string.IsNullOrEmpty(postId) && postId.Contains("#"))
            {
                post.PostId =
                    Int64.Parse(postNode.GetAttributeValue("id", string.Empty)
                        .Replace("post", string.Empty)
                        .Replace("#", string.Empty));
            }
            else if (!string.IsNullOrEmpty(postId) && postId.Contains("post"))
            {
                var testString = postNode.GetAttributeValue("id", string.Empty)
                    .Replace("post", string.Empty);
                post.PostId = !string.IsNullOrEmpty(testString) ? Int64.Parse(testString) : 0;
            }
            else
            {
                post.PostId = 0;
            }

            var postBodyNode = postNode.Descendants("td")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("postbody"));
            if (!isSimple)
            {
                this.FixQuotes(postBodyNode);
                post.PostHtml = postBodyNode.InnerHtml;
            }
            else
            {
                var postElement = new PostElements() { InnerText = Regex.Replace(postBodyNode.InnerText, "<!--.*?-->", string.Empty, RegexOptions.Multiline), ImageUrls = new List<string>() };
                var images = postBodyNode.Descendants("img").Where(node => node.GetAttributeValue("class", string.Empty) != "av");
                foreach (var image in images)
                {
                    var src = image.Attributes["src"].Value;
                    if (src.Contains("somethingawful.com"))
                        continue;
                    if (src.Contains("emoticons"))
                        continue;
                    if (src.Contains("smilies"))
                        continue;
                    postElement.ImageUrls.Add(image.Attributes["src"].Value);
                }
                post.PostElements = postElement;
            }

            HtmlNode profileLinksNode =
                    postNode.Descendants("td")
                        .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("postlinks"));
            HtmlNode postRow =
                postNode.Descendants("tr").FirstOrDefault();

            if (postRow != null)
            {
                post.HasSeen = postRow.GetAttributeValue("class", string.Empty).Contains("seen");
            }

            post.User.IsCurrentUserPost =
                profileLinksNode.Descendants("img")
                    .FirstOrDefault(node => node.GetAttributeValue("alt", string.Empty).Equals("Edit")) != null;
        }

        private void FixQuotes(HtmlNode postNode)
        {
            var quoteList =
                postNode.Descendants("a")
                    .Where(node => node.GetAttributeValue("class", string.Empty).Contains("quote_link"));
            foreach (var quote in quoteList)
            {
                int postId = ParseInt(quote.GetAttributeValue("href", string.Empty));
                quote.Attributes.Remove("href");
                quote.Attributes.Append("href", "javascript:void(0)");
                string postIdFormat = string.Concat("'#postId", postId, "'");
                quote.Attributes.Add("onclick", $"window.ForumCommand('scrollToPost', '{postId}')");
            }
        }

        private int ParseInt(string postClass)
        {
            string re1 = ".*?"; // Non-greedy match on filler
            string re2 = "(\\d+)"; // Integer Number 1

            var r = new Regex(re1 + re2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = r.Match(postClass);
            if (!m.Success) return 0;
            String int1 = m.Groups[1].ToString();
            return Convert.ToInt32(int1);
        }

        private static void ParseFromThread(Thread threadEntity, HtmlDocument threadDocument)
        {
            var title = threadDocument.DocumentNode.Descendants("title").FirstOrDefault();

            if (title != null)
            {
                threadEntity.Name = WebUtility.HtmlDecode(title.InnerText.Replace(" - The Something Awful Forums", string.Empty));
            }

            var threadIdNode = threadDocument.DocumentNode.Descendants("body").First();
            threadEntity.ThreadId = Convert.ToInt64(threadIdNode.GetAttributeValue("data-thread", string.Empty));

            threadEntity.Location = string.Format(EndPoints.ThreadPage, threadEntity.ThreadId);
            var pageNavigationNode = threadDocument.DocumentNode.Descendants("div").FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("pages top"));
            if (string.IsNullOrWhiteSpace(pageNavigationNode.InnerHtml))
            {
                threadEntity.TotalPages = 1;
                threadEntity.CurrentPage = 1;
            }
            else
            {
                var lastPageNode = pageNavigationNode.Descendants("a").FirstOrDefault(node => node.GetAttributeValue("title", string.Empty).Equals("Last page"));
                if (lastPageNode != null)
                {
                    string urlHref = lastPageNode.GetAttributeValue("href", string.Empty);
                    var query = Extensions.ParseQueryString(new Uri(urlHref).Query);
                    threadEntity.TotalPages = Convert.ToInt32(query["pagenumber"]);
                }

                var pageSelector = pageNavigationNode.Descendants("select").FirstOrDefault();

                var selectedPage = pageSelector.Descendants("option")
                    .FirstOrDefault(node => node.GetAttributeValue("selected", string.Empty).Equals("selected"));

                threadEntity.CurrentPage = Convert.ToInt32(selectedPage.GetAttributeValue("value", string.Empty));
            }
        }
    }
}
