using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Core.Managers;
using AwfulRedux.Core.Models.PostIcons;
using AwfulRedux.Core.Models.Threads;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using Template10.Mvvm;
using Thread = AwfulRedux.UI.Models.Threads.Thread;

namespace AwfulRedux.ViewModels
{
    public class NewThreadViewModel : ViewModelBase
    {
        public SmiliesViewModel SmiliesViewModel { get; set; }
        public PreviewViewModel PreviewViewModel { get; set; }
        public PostIconViewModel PostIconViewModel { get; set; }

        public async void OpenSmiliesView()
        {
            await SmiliesViewModel.LoadSmilies();
            SmiliesViewModel.IsOpen = true;
        }

        public async void OpenPostIconView()
        {
            await PostIconViewModel.Initialize(Selected.ForumId);
            PostIconViewModel.IsOpen = true;
        }

        private string _title = default(string);

        public string Title
        {
            get { return _title; }
            set
            {
                Set(ref _title, value);
            }
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

        private TextBox _subject = default(TextBox);

        public TextBox Subject
        {
            get { return _subject; }
            set
            {
                Set(ref _subject, value);
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

        private Forum _selected = default(Forum);

        public Forum Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        private NewThread _newThread;

        private readonly ThreadManager _threadManager = new ThreadManager(Views.Shell.Instance.ViewModel.WebManager);

        public override async void OnNavigatedTo(object parameter, NavigationMode mode,
            IDictionary<string, object> state)
        {
            Selected = JsonConvert.DeserializeObject<Forum>(parameter.ToString());
            Views.Shell.ShowBusy(true, "Preparing for new thread...");
            Title = "New Thread - " + Selected.Name;
            _newThread = await _threadManager.GetThreadCookiesAsync(Selected.ForumId);
            Views.Shell.ShowBusy(false);
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

        public async Task CreateThread()
        {
            if (string.IsNullOrEmpty(ReplyBox.Text) || string.IsNullOrEmpty(Subject.Text) || _newThread == null) return;
            _newThread.Content = ReplyBox.Text;
            _newThread.Subject = Subject.Text;
            _newThread.PostIcon = PostIconViewModel.PostIcon;
            _newThread.ForumId = Selected.ForumId;
            var result = await _threadManager.CreateNewThreadAsync(_newThread);
            if (result.IsSuccess)
            {
                Template10.Common.BootStrapper.Current.NavigationService.GoBack();
                return;
            }

            // TODO: Add error message when something screws up.
        }

        public async Task PreviewThread()
        {
            if (string.IsNullOrEmpty(ReplyBox.Text) || _newThread == null) return;
            _newThread.Content = ReplyBox.Text;
            _newThread.Subject = Subject.Text;
            _newThread.PostIcon = PostIconViewModel.PostIcon;
            _newThread.ForumId = Selected.ForumId;
            PreviewViewModel.IsOpen = true;
            var result = await _threadManager.CreateNewThreadPreview(_newThread);
            var post = JsonConvert.DeserializeObject<Post>(result.ResultJson);
            PreviewViewModel.LoadPost(new Thread(), post);
        }

        public async Task AddImageViaImgur()
        {
            IsLoading = true;
            Views.Shell.ShowBusy(true, "Uploading image...");
            await AddImage.AddImageViaImgur(ReplyBox);
            Views.Shell.ShowBusy(false);
            IsLoading = false;
        }
    }
}
