using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Core.Models.Messages;
using AwfulRedux.Tools.ScrollingCollection;
using AwfulRedux.Views;
using Newtonsoft.Json;
using Template10.Mvvm;
using PrivateMessage = AwfulRedux.UI.Models.Messages.PrivateMessage;

namespace AwfulRedux.ViewModels
{
    public class PrivateMessagesListViewModel : ViewModelBase
    {
        public PrivateMessageScrollingCollection PrivateMessageScrollingCollection { get; set; }

        private PrivateMessage _selected = default(PrivateMessage);

        public PrivateMessage Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
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

        public void Refresh()
        {
            PrivateMessageScrollingCollection = new PrivateMessageScrollingCollection();
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode,
           IDictionary<string, object> state)
        {
            if (state.ContainsKey(nameof(Selected)))
            {
                if (Selected == null)
                {
                    Selected = JsonConvert.DeserializeObject<PrivateMessage>(state[nameof(Selected)]?.ToString());
                    state.Clear();
                }
            }

            if (mode == NavigationMode.Forward | mode == NavigationMode.Back)
            {
                return;
            }
            PrivateMessageScrollingCollection = new PrivateMessageScrollingCollection();
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                state[nameof(Selected)] = JsonConvert.SerializeObject(Selected);
            }
            return Task.CompletedTask;
        }

        public void CreateNewPm()
        {
            Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(NewPrivateMessagePage),
    JsonConvert.SerializeObject(new NewPrivateMessage()
    {
    }));
        }
    }
}
