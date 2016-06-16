using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;
using AwfulRedux.UI;
using AwfulRedux.UI.Models.Polls;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using HtmlAgilityPack;

namespace AwfulRedux.Tools.Web
{
    public static class HtmlFormater
    {
        public static async Task<string> FormatPostHtml(Post forumPostEntity)
        {
            return string.Format("<div class=\"postbody\">{0}</div>", forumPostEntity.PostHtml);
        }

        private static bool UserNameTest(string loggedInUserName)
        {
            return loggedInUserName != "Testy Susan" || !string.IsNullOrEmpty(loggedInUserName);
        }

        public static async Task<string> FormatSaclopediaEntry(string body, PlatformIdentifier platformIdentifier)
        {
            string html = await PathIO.ReadTextAsync("ms-appx:///Assets/Website/saclopedia.html");

            var doc2 = new HtmlDocument();

            doc2.LoadHtml(html);

            HtmlNode head = doc2.DocumentNode.Descendants("head").FirstOrDefault();

            switch (platformIdentifier)
            {
                case PlatformIdentifier.WindowsPhone:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/WindowsPhone-Default.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
            }

            HtmlNode bodyNode = doc2.DocumentNode.Descendants("body").FirstOrDefault();

            bodyNode.InnerHtml = WebUtility.HtmlDecode(body);

            return doc2.DocumentNode.OuterHtml;
        }

        public static string FormatVotePoll(PollGroup pollGroup)
        {
            string poll = "<div class=\"col-lg-12\">"
                + "<div class=\"panel panel-primary\">"
                + "<div class=\"panel-heading\">"
                + "<h3 class=\"panel-title\">"
                + "<span class=\"glyphicon glyphicon-circle-arrow-right\">" + pollGroup.Title + "</span></h3></div>"
                + "<div class=\"panel-body\">"
                + "<ul class=\"list-group\">";

            foreach (var item in pollGroup.PollList)
            {
                poll += "<li class=\"list-group-item\"><div class=\"checkbox\"><label><input type=\"checkbox\">" +
                        item.Title + "</label></div></li>";
            }
               poll += "</ul></div>" + "<div class=\"panel-footer text-center\">" +
                    "<button class=\"btn btn-primary btn-block btn-sm\">Vote</button>"
                    + "<a class=\"small\" href=\"#\"</a></div></div></div>";


            return poll;
        }

        public static async Task<string> FormatPreviewHtml(Thread forumThreadEntity, Post post, PlatformIdentifier platformIdentifier)
        {
            string html = await PathIO.ReadTextAsync("ms-appx:///Assets/Website/thread.html");

            var doc2 = new HtmlDocument();

            doc2.LoadHtml(html);


            HtmlNode head = doc2.DocumentNode.Descendants("head").FirstOrDefault();

            HtmlNode body = doc2.DocumentNode.Descendants("body").FirstOrDefault();

            body.Attributes.Add("data-show-embedded-tweets", App.Settings.ShowEmbeddedTweets.ToString());
            body.Attributes.Add("data-show-embedded-gifv", App.Settings.ShowEmbeddedGifv.ToString());
            body.Attributes.Add("data-show-embedded-video", App.Settings.ShowEmbeddedVideo.ToString());

            body.Attributes.Add("data-thread-id", forumThreadEntity.ThreadId.ToString());
            body.Attributes.Add("data-thread-name", forumThreadEntity.Name);
            switch (platformIdentifier)
            {
                case PlatformIdentifier.WindowsPhone:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/WindowsPhone-Default.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
            }

            switch (forumThreadEntity.ForumId)
            {
                case 219:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/219.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
                case 26:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/26.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
                case 267:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/267.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
                case 268:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/268.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
            }

            HtmlNode bodyNode = doc2.DocumentNode.Descendants("div").FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("row clearfix"));

            string threadHtml = string.Empty;
            var postEntities = new List<Post>()
            {
                post
            };
            threadHtml += ParsePosts(0, postEntities.Count, postEntities, forumThreadEntity.IsPrivateMessage, true, true, false);

            bodyNode.InnerHtml = threadHtml;


            var images = bodyNode.Descendants("img").Where(node => node.GetAttributeValue("class", string.Empty) != "av");
            foreach (var image in images)
            {
                image.Attributes.Remove("class");
                image.Attributes.Add("class", "img-responsive");
                var src = image.Attributes["src"].Value;
                if (Path.GetExtension(src) != ".gif")
                    continue;
                if (src.Contains("somethingawful.com"))
                    continue;
                if (src.Contains("emoticons"))
                    continue;
                if (src.Contains("smilies"))
                    continue;
                image.Attributes.Add("data-gifffer", image.Attributes["src"].Value);
                image.Attributes.Remove("src");
            }
            return doc2.DocumentNode.OuterHtml;
        }

        public static async Task<string> FormatThreadHtml(Thread forumThreadEntity, List<Post> postEntities, PlatformIdentifier platformIdentifier, bool isLoggedIn = false, bool isPreviousPosts = false)
        {
            isLoggedIn = UserNameTest(forumThreadEntity.LoggedInUserName);

            string html = await PathIO.ReadTextAsync("ms-appx:///Assets/Website/thread.html");

            var doc2 = new HtmlDocument();

            doc2.LoadHtml(html);


            HtmlNode head = doc2.DocumentNode.Descendants("head").FirstOrDefault();

            HtmlNode body = doc2.DocumentNode.Descendants("body").FirstOrDefault();

            body.Attributes.Add("data-show-embedded-tweets", App.Settings.ShowEmbeddedTweets.ToString());
            body.Attributes.Add("data-show-embedded-gifv", App.Settings.ShowEmbeddedGifv.ToString());
            body.Attributes.Add("data-show-embedded-video", App.Settings.ShowEmbeddedVideo.ToString());

            body.Attributes.Add("data-thread-id", forumThreadEntity.ThreadId.ToString());
            body.Attributes.Add("data-thread-name", forumThreadEntity.Name);
            switch (platformIdentifier)
            {
                // If on dark theme, load dark theme twitter css too.
                case PlatformIdentifier.WindowsPhone:
                    head.InnerHtml += "<meta name=\"twitter:widgets:theme\" content=\"dark\"><link href=\"ms-appx-web:///Assets/Website/CSS/WindowsPhone-Default.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
            }

            switch (forumThreadEntity.ForumId)
            {
                case 219:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/219.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
                case 26:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/26.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
                case 267:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/267.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
                case 268:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/268.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
            }

            HtmlNode bodyNode = doc2.DocumentNode.Descendants("div").FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("row clearfix"));

            if (postEntities == null) return WebUtility.HtmlDecode(WebUtility.HtmlDecode(doc2.DocumentNode.OuterHtml));

            string threadHtml = string.Empty;

            if (UserNameTest(forumThreadEntity.LoggedInUserName))
            {
                threadHtml += $"<div style=\"display:none;\" id=\"loggedinusername\">{forumThreadEntity.LoggedInUserName}</div>";
            }

            threadHtml += $"<div style=\"display:none;\" id=\"scrolltopoststring\">{forumThreadEntity.ScrollToPostString}</div>";

            if (forumThreadEntity.Poll != null)
            {
                //threadHtml += FormatVotePoll(forumThreadEntity.Poll);
            }

            if (forumThreadEntity.ScrollToPost > 1 && (forumThreadEntity.ScrollToPost < postEntities.Count))
            {
                threadHtml += "<div><div id=\"showPosts\">";

                var clickHandler = $"window.ForumCommand('showPosts', '{"true"}')";

                string showThreadsButton = HtmlButtonBuilder.CreateSubmitButton(
                    $"Show {forumThreadEntity.ScrollToPost} Previous Posts", clickHandler, "showHiddenPostsButton", false);

                threadHtml += showThreadsButton;

                threadHtml += "</div><div style=\"display: none;\" id=\"hiddenPosts\">";
                threadHtml += ParsePosts(0, forumThreadEntity.ScrollToPost, postEntities, forumThreadEntity.IsPrivateMessage, isLoggedIn, false, isPreviousPosts);
                threadHtml += "</div>";
                threadHtml += ParsePosts(forumThreadEntity.ScrollToPost, postEntities.Count, postEntities, forumThreadEntity.IsPrivateMessage, isLoggedIn, false, isPreviousPosts);
            }
            else
            {
                threadHtml += ParsePosts(0, postEntities.Count, postEntities, forumThreadEntity.IsPrivateMessage, isLoggedIn, false, isPreviousPosts);
            }

            bodyNode.InnerHtml = threadHtml;

            

            var images = bodyNode.Descendants("img").Where(node => node.GetAttributeValue("class", string.Empty) != "av");
            foreach (var image in images)
            {
                image.Attributes.Remove("class");
                var src = image.Attributes["src"].Value;
                if (Path.GetExtension(src) != ".gif")
                {
                    image.Attributes.Add("class", "img-responsive");
                    continue;
                }
                if (src.Contains("somethingawful.com"))
                    continue;
                if (src.Contains("emoticons"))
                    continue;
                if (src.Contains("smilies"))
                    continue;
                image.Attributes.Add("class", "img-responsive");
                image.Attributes.Add("data-gifffer", image.Attributes["src"].Value);
                image.Attributes.Remove("src");
            }
            return doc2.DocumentNode.OuterHtml;
        }

        private static string ParsePosts(int startingCount, int endCount, List<Post> postEntities, bool isPrivateMessage, bool isLoggedIn, bool isPreview = false, bool isPreviousPosts = false)
        {
            int seenCount = 1;
            string threadHtml = string.Empty;
            for (int index = startingCount; index < endCount; index++)
            {
                Post post;
                try
                {
                    post = postEntities[index];
                }
                catch (Exception)
                {
                    // Failed to parse post, continue.
                    continue;
                }
                if (seenCount > 2)
                    seenCount = 1;
                string hasSeen = post.HasSeen ? string.Concat("seen", seenCount) : string.Concat("postCount", seenCount);
                seenCount++;
                string userAvatar = string.Empty;
                string userInfo = string.Empty;

                if (!isPreview)
                {
                    if (!string.IsNullOrEmpty(post.User.AvatarLink))
                        userAvatar = string.Concat("<img data-user-id=\"", post.User.Id, "\" src=\"", post.User.AvatarLink,
                            "\" alt=\"\" class=\"av\" border=\"0\">");
                    string username =
                        $"<p style=\"padding: 0;\" class=\"text article-title win-type-ellipsis\"><span class=\"{post.User.Roles}\">{post.User.Username}</span></p>";
                    string postData =
                        $"<p class=\"text article-title win-type-caption-alt\"><span class=\"registered\">{post.PostDate}</span></p>";
                    string userInfoClass = string.IsNullOrEmpty(post.User.AvatarLink) ? "" : "userinfo";
                    userInfo = $"<div class=\"{userInfoClass}\">{username}{postData}</div>";
                }

                string postButtons;
                if (isPreviousPosts)
                {
                    postButtons = CreatePreviousButtons(post);
                }
                else
                {
                    postButtons = isLoggedIn && !isPreview ? CreateButtons(post) : string.Empty;
                }

                string footer = string.Empty;
                if (!isPrivateMessage)
                {
                    footer = $"<tr class=\"postbar\"><td class=\"postlinks\">{postButtons}</td></tr>";
                }
                string postBody = string.Format("<div id=\"{1}\" class=\"postbody\">{0}</div>", post.PostHtml, post.PostId);
                string padding = isPreview ? "" : "padding: 15px;";
                string threadView = isPreview ? "" : "threadView";
                threadHtml +=
                    string.Format(
                        "<div class={6} id={4}>" +
                        "<div id={5}>" +
                        "<div class=\"row clearfix\">" +
                        "<div class=\"col-md-4\">" +
                        "<div id=\"{8}\">" +
                        "{0}{1}" +
                        "</div>" +
                        "</div>" +
                        "<div style=\"{7}\" class=\"col-md-8\">" +
                        "<div class=\"article-content\">" +
                        "{2}" +
                        "<footer>{3}</footer>" +
                        "</div>" +
                        "</div>" +
                        "</div>" +
                        "</div>" +
                        "</div>",
                        userAvatar, userInfo, postBody, footer, string.Concat("\"pti", index + 1, "\""), string.Concat("\"postId", post.PostId, "\""), string.Concat("\"", hasSeen, "\""), padding, threadView);
            }
            return threadHtml;
        }

        private static string CreatePreviousButtons(Post post)
        {
            var clickHandler = $"window.QuotePreviousPost('{post.PostId}')";

            string quoteButton = HtmlButtonBuilder.CreateSubmitButton("Add Quote", clickHandler, string.Empty);

            return string.Concat("<ul class=\"profilelinks\">",
                       quoteButton, "</ul>");
        }

        private static string CreateButtons(Post post)
        {
            var clickHandler = $"window.QuotePost('{post.PostId}')";

            string quoteButton = HtmlButtonBuilder.CreateSubmitButton("Quote", clickHandler, string.Empty);

            clickHandler = $"window.EditPost('{post.PostId}')";

            string editButton = HtmlButtonBuilder.CreateSubmitButton("Edit", clickHandler, string.Empty);

            clickHandler = $"window.MarkAsLastRead('{post.PostIndex}')";

            string markAsLastReadButton = HtmlButtonBuilder.CreateSubmitButton("Last Read", clickHandler, string.Empty);

            return post.User.IsCurrentUserPost
                    ? string.Concat("<ul class=\"profilelinks\">",
                        quoteButton, markAsLastReadButton, editButton, "</ul>")
                    : string.Concat("<ul class=\"profilelinks\">",
                        quoteButton, markAsLastReadButton, "</ul>");
        }
    }
}
