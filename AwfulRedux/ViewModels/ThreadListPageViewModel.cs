﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using AmazingPullToRefresh.Controls;
using AwfulRedux.Controls;
using AwfulRedux.Core.Managers;
using AwfulRedux.Tools.ScrollingCollection;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.Views;
using Kimono.Controls;
using Newtonsoft.Json;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class ThreadListPageViewModel : ForumThreadListBaseViewModel
    {
        private PageScrollingCollection _ForumPageScrollingCollection = default(PageScrollingCollection);
        public PageScrollingCollection ForumPageScrollingCollection
        {
            get { return _ForumPageScrollingCollection; }
            set
            {
                Set(ref _ForumPageScrollingCollection, value);
            }
        }
        private Forum _forum = default(Forum);
        public Forum Forum
        {
            get { return _forum; }
            set
            {
                Set(ref _forum, value);
            }
        }

        public async void PullToRefresh_ListView(object sender, RefreshRequestedEventArgs e)
        {
            Refresh();
        }

        public override async void OnNavigatedTo(object parameter, NavigationMode mode,
            IDictionary<string, object> state)
        {
            if (WebManager == null)
            {
                await LoginUser();
            }
            Template10.Common.BootStrapper.Current.NavigationService.FrameFacade.BackRequested += MasterDetailViewControl.NavigationManager_BackRequested;
            if (Forum != null && (mode == NavigationMode.Forward | mode == NavigationMode.Back))
            {
                return;
            }

            if (state.ContainsKey(nameof(Forum)))
            {
                if (Forum == null)
                {
                    Forum = JsonConvert.DeserializeObject<Forum>(state[nameof(Forum)]?.ToString());
                }
            }
            else
            {
                var forum = JsonConvert.DeserializeObject<Forum>((string)parameter);
                if (forum == null) return;
                Forum = forum;
            }

            ForumPageScrollingCollection = new PageScrollingCollection(Forum, 1);
            ForumPageScrollingCollection.CheckIsPaywallEvent += ForumPageScrollingCollection_CheckIsPaywallEvent;

            if (state.ContainsKey(nameof(Selected)))
            {
                if (Selected == null)
                {
                    Selected = JsonConvert.DeserializeObject<Thread>(state[nameof(Selected)]?.ToString());
                    await ThreadView.LoadThread(Selected);
                }
            }

            state.Clear();
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            Template10.Common.BootStrapper.Current.NavigationService.FrameFacade.BackRequested -= MasterDetailViewControl.NavigationManager_BackRequested;
            if (suspending)
            {
                if (Selected != null)
                {
                    var html = Selected.Html;
                    var posts = Selected.Posts;
                    Selected.Html = null;
                    Selected.Posts = null;
                    state[nameof(Selected)] = JsonConvert.SerializeObject(Selected);
                    Selected.Html = html;
                    Selected.Posts = posts;
                }
                state[nameof(Forum)] = JsonConvert.SerializeObject(Forum);
            }
            return Task.CompletedTask;
        }

        public void Refresh()
        {
            ForumPageScrollingCollection = new PageScrollingCollection(Forum, 1);
        }

        private void ForumPageScrollingCollection_CheckIsPaywallEvent(object sender, PageScrollingCollection.IsPaywallArgs e)
        {
            if (!e.IsPaywall) return;
            NavigationService.Navigate(typeof (PaywallPage));
            var length = NavigationService.Frame.BackStack.Count;
            NavigationService.Frame.BackStack.RemoveAt(length - 1);
        }

        public void ReplyToThread(Thread thread)
        {
            NavigationService.Navigate(typeof(ReplyPage), JsonConvert.SerializeObject(thread));
        }

        public void CreateThread()
        {
            if (Views.Shell.Instance.ViewModel.IsLoggedIn)
            {
                NavigationService.Navigate(typeof(NewThreadPage), JsonConvert.SerializeObject(Forum));
            }
            else
            {
                // TODO: Figure out what to show here?
            }
        }
    }
}
