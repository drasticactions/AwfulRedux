using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class ReplyThreadViewModel : ViewModelBase
    {
        public SmiliesViewModel SmiliesViewModel { get; set; }
        private string _title = default(string);

        public string Title
        {
            get { return _title; }
            set
            {
                Set(ref _title, value);
            }
        }

        private string _postBody = default(string);

        public string PostBody
        {
            get { return _postBody; }
            set
            {
                Set(ref _postBody, value);
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

        public async void OpenSmiliesView()
        {
            await SmiliesViewModel.LoadSmilies();
            IsOpen = true;
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

        public override async void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            base.OnNavigatedTo(parameter, mode, state);
            Selected = JsonConvert.DeserializeObject<ThreadReply>(parameter.ToString());
            if (Selected.QuoteId > 0)
            {
                Title = "Quote - " + Selected.Thread.Name;
            }
            else
            {
                Title = "Reply - " + Selected.Thread.Name;
            }
        }
    }
}
