using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AwfulForumsLibrary.Managers;
using AwfulForumsLibrary.Models.Replies;
using AwfulRedux.Tools.Authentication;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class PreviousPostsViewModel : ViewModelBase
    {
        public TextBox ReplyBox { get; set; }

        private bool _isLoading = default(bool);

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }

        private string _postHtml = default(string);

        public string PostHtml
        {
            get { return _postHtml; }
            set
            {
                Set(ref _postHtml, value);
            }
        }

        private bool _isOpen = default(bool);

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                Set(ref _isOpen, value);
            }
        }

        private PlatformIdentifier GetTheme
        {
            get
            {
                if (App.Settings.AppTheme == ApplicationTheme.Light)
                {
                    return PlatformIdentifier.Windows8;
                }

                return PlatformIdentifier.WindowsPhone;
            }
        }

        private WebManager _webManager;

        private ReplyManager _replyManager;

        public async Task LoginUser()
        {
            var cookie = await LoginHelper.LoginDefaultUser();
            _webManager = new WebManager(cookie);
            _replyManager = new ReplyManager(_webManager);
        }

        public async void LoadPreviousPosts(Thread thread, string posts)
        {
            if (!string.IsNullOrEmpty(PostHtml)) return;
            if (_replyManager == null)
            {
                await LoginUser();
            }
            var newPosts = JsonConvert.DeserializeObject<List<Post>>(posts);
            //PostHtml = await HtmlFormater.FormatThreadHtml(thread, newPosts, GetTheme, true, true);
        }

        public async void AddQuoteString(long postId)
        {
            if (_replyManager == null)
            {
                await LoginUser();
            }
            var quoteString = await _replyManager.GetQuoteString(postId);
            ReplyBox.Text += Environment.NewLine + quoteString + Environment.NewLine;
        }
    }
}
