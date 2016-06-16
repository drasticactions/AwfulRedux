using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Core.Managers;
using AwfulRedux.Core.Models.Replies;
using AwfulRedux.Core.Models.Web;
using AwfulRedux.Database;
using AwfulRedux.Tools.Authentication;
using AwfulRedux.Tools.Database;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using SQLite.Net.Platform.WinRT;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class ReplyThreadViewModel : ViewModelBase
    {
        private readonly AuthenticatedUserDatabase _udb = new AuthenticatedUserDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("ForumsRedux.db"));

        public SmiliesViewModel SmiliesViewModel { get; set; }
        public PreviewViewModel PreviewViewModel { get; set; }
        public PreviousPostsViewModel PreviousPostsViewModel { get; set; }
        private string _title = default(string);

        public string Title
        {
            get { return _title; }
            set
            {
                Set(ref _title, value);
            }
        }

        private TextBox _replyBox = default(TextBox);

        public TextBox ReplyBox
        {
            get { return _replyBox; }
            set
            {
                Set(ref _replyBox, value);
            }
        }

        public async void OpenSmiliesView()
        {
            await SmiliesViewModel.LoadSmilies();
            SmiliesViewModel.IsOpen = true;
        }

        private bool _isLoading = default(bool);

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }

        private ThreadReply _selected = default(ThreadReply);

        public ThreadReply Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        private ForumReply _forumReply { get; set; }

        private WebManager _webManager;

        private ReplyManager _replyManager;

        public async Task LoginUser()
        {
            var cookie = await LoginHelper.LoginDefaultUser();
            _webManager = new WebManager(cookie);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            await base.OnNavigatedToAsync(parameter, mode, suspensionState);
            if (_replyManager == null)
            {
                await LoginUser();
                _replyManager = new ReplyManager(_webManager);
            }
            Selected = JsonConvert.DeserializeObject<ThreadReply>(parameter.ToString());
            //Views.Shell.ShowBusy(true, "Preparing thread...");
            if (Selected.IsEdit)
            {
                Title = "Edit - " + Selected.Thread.Name;
                _forumReply = await _replyManager.GetReplyCookiesForEdit(Selected.QuoteId);
                ReplyBox.Text = _forumReply.Quote;
            }
            else if (Selected.QuoteId > 0)
            {
                Title = "Quote - " + Selected.Thread.Name;
                _forumReply = await _replyManager.GetReplyCookies(0, Selected.QuoteId);
                ReplyBox.Text = _forumReply.Quote;
            }
            else
            {
                Title = "Reply - " + Selected.Thread.Name;
                _forumReply = await _replyManager.GetReplyCookies(Selected.Thread.ThreadId);
            }
            //Views.Shell.ShowBusy(false);
        }

        public void SelectBbCode(object sender, RoutedEventArgs e)
        {
            var menuFlyoutItem = sender as MenuFlyoutItem;
            if (menuFlyoutItem == null) return;
            var code = "";
            if (menuFlyoutItem.CommandParameter != null)
            {
                switch (menuFlyoutItem.CommandParameter.ToString().ToLower())
                {
                    case "bold":
                        code = "b";
                        break;
                    case "indent":
                        code = "i";
                        break;
                    case "strike":
                        code = "s";
                        break;
                    case "spoiler":
                        code = "spoiler";
                        break;
                    case "quote":
                        code = "quote";
                        break;
                }
            }

            if (!string.IsNullOrEmpty(ReplyBox.SelectedText))
            {
                string selectedText = "[{0}]" + ReplyBox.SelectedText + "[/{0}]";
                ReplyBox.SelectedText = string.Format(selectedText, code);
            }
            else
            {
                string text = string.Format("[{0}][/{0}]", code);
                string replyText = string.IsNullOrEmpty(ReplyBox.Text) ? string.Empty : ReplyBox.Text;
                if (replyText != null) ReplyBox.Text = replyText.Insert(ReplyBox.SelectionStart, text);
            }
        }

        public async Task ReplyToThread()
        {
            if (string.IsNullOrEmpty(ReplyBox.Text) || _forumReply == null) return;
            _forumReply.Message = ReplyBox.Text;
            IsLoading = true;
            var loadingString = Selected.IsEdit ? "Editing Post..." : "Posting reply (Better hope it doesn't suck...)";
            //Views.Shell.ShowBusy(true, loadingString);
            Result result;
            if (Selected.IsEdit)
            {
                result = await _replyManager.SendUpdatePost(_forumReply);
            }
            else
            {
                result = await _replyManager.SendPost(_forumReply);
            }
            //Views.Shell.ShowBusy(false);
            if (result.IsSuccess)
            {
                IsLoading = false;
                Template10.Common.BootStrapper.Current.NavigationService.GoBack();
                return;
            }
            IsLoading = false;
            // TODO: Add error message when something screws up.
        }

        public async Task PreviewPost()
        {
            IsLoading = true;
            if (string.IsNullOrEmpty(ReplyBox.Text) || _forumReply == null) return;
            _forumReply.Message = ReplyBox.Text;
            PreviewViewModel.IsOpen = true;
            var result = Selected.IsEdit
                ? await _replyManager.CreatePreviewEditPost(_forumReply)
                : await _replyManager.CreatePreviewPost(_forumReply);
            var post = JsonConvert.DeserializeObject<Post>(result.ResultJson);
            PreviewViewModel.LoadPost(Selected.Thread, post);
            IsLoading = false;
        }

        public void ShowPreviousPosts()
        {
            PreviousPostsViewModel.IsOpen = true;
            PreviousPostsViewModel.LoadPreviousPosts(Selected.Thread, JsonConvert.SerializeObject(_forumReply.ForumPosts));
        }

        public async Task AddImageViaImgur()
        {
            IsLoading = true;
            //Views.Shell.ShowBusy(true, "Uploading image...");
            await AddImage.AddImageViaImgur(ReplyBox);
            //Views.Shell.ShowBusy(false);
            IsLoading = false;
        }
    }
}
