﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using AwfulRedux.Common;
using AwfulRedux.Core.Managers;
using AwfulRedux.Core.Models.Threads;
using AwfulRedux.Core.Tools;
using AwfulRedux.Database;
using AwfulRedux.Notifications;
using AwfulRedux.Tools.Errors;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.ViewModels;
using AwfulRedux.Views;
using Newtonsoft.Json;
using Thread = AwfulRedux.Core.Models.Threads.Thread;

namespace AwfulRedux.Tools.Web
{
    public class WebViewCommands
    {
        public static void WebView_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            var command = new WebViewNotifyCommand.ThreadDomContentLoadedCommand();
            command.Execute(sender);
        }

        public class WebViewNotifyCommand
        {
            private static string _url;

            public static async void WebView_ScriptNotify(object sender, NotifyEventArgs e)
            {
                var webview = sender as WebView;
                if (webview == null)
                {
                    return;
                }

                try
                {
                    string stringJson = e.Value;
                    var command = JsonConvert.DeserializeObject<ThreadCommand>(stringJson);
                    switch (command.Command)
                    {
                        case "openLink":
                            await Windows.System.Launcher.LaunchUriAsync(new Uri(command.Id));
                            break;
                        case "userProfile":
                            //var navUser = new NavigateToUserProfilePageCommand();
                            //navUser.Execute(Convert.ToInt64(command.Id));
                            break;
                        case "downloadImage":
                            _url = command.Id;
                            var message = string.Format("Do you want to download this image?{0}{1}", Environment.NewLine,
                                command.Id);
                            var msgBox =
                                new MessageDialog(message,
                                    "Download Image");
                            var okButton = new UICommand("Yes") {Invoked = PictureOkButtonClick};
                            var cancelButton = new UICommand("No") {Invoked = PictureCancelButtonClick};
                            msgBox.Commands.Add(okButton);
                            msgBox.Commands.Add(cancelButton);
                            await msgBox.ShowAsync();
                            break;
                        case "showPosts":
                            await webview.InvokeScriptAsync("ShowHiddenPosts", new[] {string.Empty});
                            break;
                        case "scrollToDivStart":
                            await webview.InvokeScriptAsync("ScrollToDiv", new[] { command.Id });
                            break;
                        case "profile":
                            //Frame.Navigate(typeof(UserProfileView), command.Id);
                            break;
                        case "openPost":
                            try
                            {
                                var postManager = new PostManager(Views.Shell.Instance.ViewModel.WebManager);
                                var postId = ParsePostId(command.Id);
                                var result = await postManager.GetPostAsync(postId);
                                var post = JsonConvert.DeserializeObject<Post>(result.ResultJson);
                                await
                                    webview.InvokeScriptAsync("AddPostToThread",
                                        new[] {postId.ToString(), post.PostHtml});
                            }
                            catch (Exception)
                            {
                                // TODO: Throw error if it fails.
                            }
                            break;
                        case "post_history":
                            //Frame.Navigate(typeof(UserPostHistoryPage), command.Id);
                            break;
                        case "rap_sheet":
                            //Frame.Navigate(typeof(RapSheetView), command.Id);
                            break;
                        case "quote":
                            var quoteObject = JsonConvert.DeserializeObject<PostQuote>(command.Id);
                            Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(ReplyPage),
                JsonConvert.SerializeObject(new ThreadReply()
                {
                    Thread = new UI.Models.Threads.Thread()
                    {
                        ThreadId = Convert.ToInt32(quoteObject.thread_id),
                        Name = quoteObject.thread_name
                    },
                    QuoteId = Convert.ToInt32(quoteObject.post_id)
                }));
                            break;
                        case "edit":
                            var editObject = JsonConvert.DeserializeObject<PostQuote>(command.Id);
                            Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(ReplyPage),
                JsonConvert.SerializeObject(new ThreadReply()
                {
                    Thread = new UI.Models.Threads.Thread()
                    {
                        ThreadId = Convert.ToInt32(editObject.thread_id),
                        Name = editObject.thread_name
                    },
                    QuoteId = Convert.ToInt32(editObject.post_id),
                    IsEdit = true
                }));
                            break;
                        case "scrollToPost":
                            try
                            {
                                if (command.Id != null)
                                {
                                    await
                                        webview.InvokeScriptAsync("ScrollToDiv",
                                            new[] {string.Concat("#postId", command.Id)});
                                }
                            }
                            catch (Exception)
                            {
                                Debug.WriteLine("Could not scroll to post...");
                            }
                            break;
                        case "markAsLastRead":
                            try
                            {
                                var lastreadObject = JsonConvert.DeserializeObject<PostQuote>(command.Id);
                                var threadManager = new ThreadManager(Views.Shell.Instance.ViewModel.WebManager);
                                await threadManager.MarkPostAsLastReadAs(Convert.ToInt32(lastreadObject.thread_id), Convert.ToInt32(lastreadObject.post_id));
                                int nextPost = Convert.ToInt32(lastreadObject.post_id) + 1;
                                await webview.InvokeScriptAsync("ScrollToDiv", new[] { string.Concat("#postId", nextPost.ToString()) });
                                NotifyStatusTile.CreateToastNotification("Last Read", "Post marked as last read.");
                            }
                            catch (Exception ex)
                            {
                                ResultChecker.SendMessageDialogAsync("Could not mark thread as last read", false);
                            }
                            break;
                        case "setFont":
                            break;
                        case "previous":
                            var viewModel = webview.DataContext as PreviousPostsViewModel;
                            if (viewModel == null) return;
                            var quoteObject2 = JsonConvert.DeserializeObject<PostQuote>(command.Id);
                            viewModel.AddQuoteString(Convert.ToInt32(quoteObject2.post_id));
                            break;
                        case "openThread":
                            var query = Extensions.ParseQueryString(command.Id);
                            if (query["threadid"] != null)
                            {
                                var url = string.Format(EndPoints.ThreadPage, query["threadid"]);
                                var newThreadEntity = new Thread()
                                {
                                    Location = url
                                };
                                if (query["pagenumber"] != null)
                                {
                                    newThreadEntity.CurrentPage = Convert.ToInt32(query["pagenumber"]);
                                }
                                var json = JsonConvert.SerializeObject(newThreadEntity);
                                Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(ThreadPage), json);
                            }
                           

                            //if (query.ContainsKey("action") && query["action"].Equals("showPost"))
                            //{
                            //    //var postManager = new PostManager();
                            //    //var html = await postManager.GetPost(Convert.ToInt32(query["postid"]));
                            //    return;
                            //}
                            //Locator.ViewModels.ThreadPageVm.IsLoading = true;
                            //var newThreadEntity = new ForumThreadEntity()
                            //{
                            //    Location = command.Id,
                            //    ImageIconLocation = "/Assets/ThreadTags/noicon.png"
                            //};
                            //Locator.ViewModels.ThreadPageVm.ForumThreadEntity = newThreadEntity;

                            //await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();

                            //var tabManager = new MainForumsDatabase();
                            //var test2 = await tabManager.DoesTabExist(newThreadEntity);
                            //if (!test2)
                            //{
                            //    await tabManager.AddThreadToTabListAsync(newThreadEntity);
                            //}
                            //Locator.ViewModels.ThreadPageVm.LinkedThreads.Add(newThreadEntity);
                            break;
                        default:
                            var msgDlg = new MessageDialog("Not working yet!")
                            {
                                DefaultCommandIndex = 1
                            };
                            await msgDlg.ShowAsync();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            private static int ParsePostId(string txt)
            {
                const string re1 = ".*?"; // Non-greedy match on filler
                const string re2 = "\\d+"; // Uninteresting: int
                const string re3 = ".*?"; // Non-greedy match on filler
                const string re4 = "(\\d+)"; // Integer Number 1

                var r = new Regex(re1 + re2 + re3 + re4, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                var m = r.Match(txt);
                if (!m.Success) return 0;
                var int1 = m.Groups[1].ToString();
                return Convert.ToInt32(int1);
            }

            private static void PictureCancelButtonClick(IUICommand command)
            {

            }

            private static async void PictureOkButtonClick(IUICommand command)
            {
                var result = await DownloadImageAsync(_url);
                if (result)
                {
                    var msgBox = new MessageDialog("Image downloaded! Check your camera roll!", "Download Image");
                    await msgBox.ShowAsync();
                    return;
                }

                var msgBox2 = new MessageDialog("Image download failed! :(", "Download Image");
                await msgBox2.ShowAsync();
            }

            private static async Task<bool> DownloadImageAsync(string url)
            {
                try
                {
                    var fileName = Path.GetFileName(new Uri(url).AbsolutePath);
                    var client = new HttpClient();
                    var stream = await client.GetStreamAsync(url);
                    await FileAccessCommands.SaveStreamToCameraRoll(stream, fileName);
                }
                catch (Exception ex)
                {
                    //AwfulDebugger.SendMessageDialogAsync("Failed to download image", ex.InnerException);
                    return false;
                }
                return true;
            }

            public class ThreadDomContentLoadedCommand : AlwaysExecutableCommand
            {
                public async override void Execute(object parameter)
                {
                    var test = parameter as WebView;
                    if (test == null)
                    {
                        return;
                    }
                    try
                    {
                        //if (Locator.ViewModels.ThreadPageVm.ForumThreadEntity.ScrollToPost > 0)
                        //{
                        //    await test.InvokeScriptAsync("ScrollToDiv", new[] { Locator.ViewModels.ThreadPageVm.ForumThreadEntity.ScrollToPostString });
                        //}
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Webview Failer {0}", ex);
                    }
                }
            }
        }
    }

    public class PostQuote
    {
        public string post_id { get; set; }
        public string thread_id { get; set; }
        public string thread_name { get; set; }
    }
}
