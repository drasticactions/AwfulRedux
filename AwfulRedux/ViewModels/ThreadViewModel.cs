using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using AwfulRedux.Core.Managers;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class ThreadViewModel : ViewModelBase
    {

        private Thread _selected = default(Thread);

        public Thread Selected
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

        public async Task LoadThread(Thread thread)
        {
            IsLoading = true;
            var tempManager = new PostManager(Views.Shell.Instance.WebManager);
            var result = await tempManager.GetThreadPostsAsync(thread.Location, 0);
            var postresult = JsonConvert.DeserializeObject<List<Post>>(result.ResultJson);
            Selected.Posts = postresult;
            Selected.Html = await HtmlFormater.FormatThreadHtml(Selected, postresult, GetTheme);
            IsLoading = false;
        }
    }
}
