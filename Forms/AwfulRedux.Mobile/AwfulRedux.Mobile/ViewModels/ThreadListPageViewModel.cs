using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AwfulRedux.Core.Managers;
using AwfulRedux.Mobile.Tools;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using Prism.Navigation;

namespace AwfulRedux.Mobile.ViewModels
{
    public class ThreadListPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;

        public ThreadListPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public int PageCount { get; set; } = 1;

        private bool _hasMoreItems = true;

        private ThreadManager _threadManager = new ThreadManager(App.WebManager);

        public Action<Thread> ItemSelected { get; set; }

        private ObservableCollection<Thread> _threads = new ObservableCollection<Thread>();

        public ObservableCollection<Thread> Threads
        {
            get { return _threads; }
            set
            {
                SetProperty(ref _threads, value);
                OnPropertyChanged();
            }
        }

        private Thread _selectedThread;

        public Thread SelectedThread
        {
            get { return _selectedThread; }
            set
            {
                _selectedThread = value;
                if (SelectedThread == null)
                    return;
                if (ItemSelected == null)
                {
                    //page.Navigation.PushAsync(new ForumThreadPage(_selectedThread));
                    _selectedThread = null;
                }
                else
                {
                    ItemSelected.Invoke(_selectedThread);
                    _selectedThread = null;
                }
                OnPropertyChanged();
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private Forum _selectedForum;

        public Forum SelectedForum
        {
            get { return _selectedForum; }
            set
            {
                _selectedForum = value;
                OnPropertyChanged();
            }
        }

        public async Task GetThreads()
        {
            try
            {
                if (!_hasMoreItems) return;
                var result = await _threadManager.GetForumThreadsAsync(SelectedForum.Location, SelectedForum.Id, PageCount);
                var resultCheck = await ResultChecker.CheckPaywallOrSuccess(result);
                var forumThreadEntities = JsonConvert.DeserializeObject<List<Thread>>(result.ResultJson);
                foreach (var forumThreadEntity in forumThreadEntities.Where(forumThreadEntity => !forumThreadEntity.IsAnnouncement))
                {
                    Threads.Add(forumThreadEntity);
                }
                if (forumThreadEntities.Any(node => !node.IsAnnouncement))
                {
                    PageCount++;
                }
                else
                {
                    _hasMoreItems = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task Initialize()
        {
            await GetThreads();
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            SelectedForum = (Forum)parameters["forum"];
            Title = SelectedForum.Name;
            await GetThreads();
        }
    }
}
