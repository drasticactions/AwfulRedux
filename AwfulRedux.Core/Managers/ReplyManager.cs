using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.Core.Interfaces;
using AwfulRedux.Core.Models.Posts;
using AwfulRedux.Core.Models.Replies;
using AwfulRedux.Core.Models.Web;
using AwfulRedux.Core.Tools;
using HtmlAgilityPack;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace AwfulRedux.Core.Managers
{
    public class ReplyManager
    {
        private readonly IWebManager _webManager;

        public ReplyManager(IWebManager webManager)
        {
            _webManager = webManager;
        }

        public async Task<Result> GetReplyCookiesForEdit(long postId)
        {
            try
            {
                string url = string.Format(EndPoints.EditBase, postId);
                var result = await _webManager.GetData(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(result.ResultHtml);

                HtmlNode[] formNodes = doc.DocumentNode.Descendants("input").ToArray();

                HtmlNode bookmarkNode =
                    formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("bookmark"));

                HtmlNode[] textAreaNodes = doc.DocumentNode.Descendants("textarea").ToArray();

                HtmlNode textNode =
                    textAreaNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("message"));

                var threadManager = new ThreadManager(_webManager);

                //Get previous posts from quote page.
                string url2 = string.Format(EndPoints.QuoteBase, postId);
                var result2 = await _webManager.GetData(url2);
                HtmlDocument doc2 = new HtmlDocument();
                doc2.LoadHtml(result2.ResultHtml);

                var forumThreadPosts = new List<Post>();

                HtmlNode threadNode =
                   doc2.DocumentNode.Descendants("div")
                       .FirstOrDefault(node => node.GetAttributeValue("id", string.Empty).Contains("thread"));


                var postManager = new PostManager(_webManager);
                foreach (
                    HtmlNode postNode in
                        threadNode.Descendants("table")
                            .Where(node => node.GetAttributeValue("class", string.Empty).Contains("post")))
                {
                    var post = new Post();
                    postManager.ParsePost(post, postNode);
                    forumThreadPosts.Add(post);
                }

                var forumReplyEntity = new ForumReply();
                try
                {
                    string quote = WebUtility.HtmlDecode(textNode.InnerText);
                    forumReplyEntity.ForumPosts = forumThreadPosts;
                    string bookmark = bookmarkNode.OuterHtml.Contains("checked") ? "yes" : "no";
                    forumReplyEntity.MapEditPostInformation(quote, postId, bookmark);
                    result.ResultJson = JsonConvert.SerializeObject(forumReplyEntity);
                    return result;
                }
                catch (Exception)
                {
                    return result;
                }
            }
            catch (Exception)
            {
                return new Result();
            }
        }

        public async Task<Result> GetReplyCookies(long threadId = 0, long postId = 0)
        {
            if (threadId == 0 && postId == 0) return new Result();
            try
            {
                string url;
                url = threadId > 0 ? string.Format(EndPoints.ReplyBase, threadId) : string.Format(EndPoints.QuoteBase, postId);
                var result = await _webManager.GetData(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(result.ResultHtml);

                HtmlNode[] formNodes = doc.DocumentNode.Descendants("input").ToArray();

                HtmlNode formKeyNode =
                    formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("formkey"));

                HtmlNode formCookieNode =
                    formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("form_cookie"));

                HtmlNode bookmarkNode =
                    formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("bookmark"));

                HtmlNode[] textAreaNodes = doc.DocumentNode.Descendants("textarea").ToArray();

                HtmlNode textNode =
                    textAreaNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("message"));

                HtmlNode threadIdNode =
                    formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("threadid"));

                var forumThreadPosts = new List<Post>();

                HtmlNode threadNode =
                   doc.DocumentNode.Descendants("div")
                       .FirstOrDefault(node => node.GetAttributeValue("id", string.Empty).Contains("thread"));


                var postManager = new PostManager(_webManager);
                foreach (
                    HtmlNode postNode in
                        threadNode.Descendants("table")
                            .Where(node => node.GetAttributeValue("class", string.Empty).Contains("post")))
                {
                    var post = new Post();
                    postManager.ParsePost(post, postNode);
                    forumThreadPosts.Add(post);
                }

                var forumReplyEntity = new ForumReply();
                try
                {
                    string formKey = formKeyNode.GetAttributeValue("value", "");
                    string formCookie = formCookieNode.GetAttributeValue("value", "");
                    string quote = WebUtility.HtmlDecode(textNode.InnerText);
                    string threadIdTest = threadIdNode.GetAttributeValue("value", "");
                    forumReplyEntity.MapThreadInformation(formKey, formCookie, quote, threadIdTest);
                    forumReplyEntity.ForumPosts = forumThreadPosts;
                    result.ResultJson = JsonConvert.SerializeObject(forumReplyEntity);
                    return result;
                }
                catch (Exception)
                {
                    return result;
                }
            }
            catch (Exception)
            {
                return new Result();
            }
        }
    }
}
