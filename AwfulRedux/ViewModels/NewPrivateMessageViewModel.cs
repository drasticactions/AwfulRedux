﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Core.Managers;
using AwfulRedux.Core.Models.Messages;
using AwfulRedux.Core.Models.PostIcons;
using AwfulRedux.Core.Models.Web;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using Template10.Mvvm;
using PrivateMessage = AwfulRedux.UI.Models.Messages.PrivateMessage;

namespace AwfulRedux.ViewModels
{
    public class NewPrivateMessageViewModel : ViewModelBase
    {
        public SmiliesViewModel SmiliesViewModel { get; set; }

        public PostIconViewModel PostIconViewModel { get; set; }

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

        private TextBox _subject = default(TextBox);

        public TextBox Subject
        {
            get { return _subject; }
            set
            {
                Set(ref _subject, value);
            }
        }

        private TextBox _recipient = default(TextBox);

        public TextBox Recipient
        {
            get { return _recipient; }
            set
            {
                Set(ref _recipient, value);
            }
        }

        private PrivateMessage _selected = default(PrivateMessage);

        public PrivateMessage Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        private NewPrivateMessage _newPrivateMessage = new NewPrivateMessage();

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoading = true;
            await base.OnNavigatedToAsync(parameter, mode, suspensionState);
            Selected = JsonConvert.DeserializeObject<PrivateMessage>(parameter.ToString());
            if (!string.IsNullOrEmpty(Selected.Title))
            {
                Title = "Re: " + Selected.Title;
                Subject.Text = "Re: " + Selected.Title;
            }
            else
            {
                Title = "New Private Message";
            }

            if (!string.IsNullOrEmpty(Selected.Sender))
            {
                Recipient.Text = Selected.Sender;
            }
            IsLoading = false;
        }

        public async Task CreatePm()
        {
            IsLoading = true;
            if (string.IsNullOrEmpty(ReplyBox.Text) || _newPrivateMessage == null) return;
            if (PostIconViewModel.PostIcon == null) return;
            IsLoading = true;
            Result result = new Result();
            try
            {
                _newPrivateMessage.Icon = PostIconViewModel.PostIcon;
                _newPrivateMessage.Body = ReplyBox.Text;
                _newPrivateMessage.Receiver = Recipient.Text;
                _newPrivateMessage.Title = Subject.Text;
                result = await _postManager.SendPrivateMessageAsync(_newPrivateMessage);
            }
            catch (Exception)
            {
                // TODO: Show error.
            }
            IsLoading = false;
            if (result.IsSuccess)
            {
                Template10.Common.BootStrapper.Current.NavigationService.GoBack();
                IsLoading = false;
                return;
            }
            IsLoading = false;
            // TODO: Add error message when something screws up.
        }

        private readonly PrivateMessageManager _postManager = new PrivateMessageManager(Views.Shell.Instance.ViewModel.WebManager);

        public async void OpenPostIconView()
        {
            await PostIconViewModel.Initialize(1);
            PostIconViewModel.IsOpen = true;
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

        public async Task AddImageViaImgur()
        {
            IsLoading = true;
            await AddImage.AddImageViaImgur(ReplyBox);
            IsLoading = false;
        }
    }
}
