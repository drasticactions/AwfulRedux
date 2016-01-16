using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Tools.ScrollingCollection;
using AwfulRedux.UI.Models.Messages;
using Newtonsoft.Json;
using Template10.Mvvm;

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
            PrivateMessageScrollingCollection.Clear();
            PrivateMessageScrollingCollection.HasMoreItems = true;
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode,
           IDictionary<string, object> state)
        {
            if (mode == NavigationMode.Forward | mode == NavigationMode.Back)
            {
                return;
            }
            PrivateMessageScrollingCollection = new PrivateMessageScrollingCollection();
        }
    }
}
