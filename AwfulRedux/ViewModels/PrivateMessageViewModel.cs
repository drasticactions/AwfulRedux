using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using AwfulRedux.Core.Managers;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI;
using AwfulRedux.UI.Models.Messages;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class PrivateMessageViewModel : ViewModelBase
    {
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

        private string _html = default(string);

        public string Html
        {
            get { return _html; }
            set
            {
                Set(ref _html, value);
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

        private readonly PrivateMessageManager _postManager = new PrivateMessageManager(Views.Shell.Instance.ViewModel.WebManager);

        public async Task LoadPrivateMessage()
        {
            IsLoading = true;
            var result = await _postManager.GetPrivateMessageAsync(Selected.MessageUrl);
            if (!result.IsSuccess)
            {
                IsLoading = false;
                return;
                // TODO: Show error.
            }

            var postresult = JsonConvert.DeserializeObject<Post>(result.ResultJson);

            await FormatPmHtml(postresult);
            IsLoading = false;
        }

        private async Task FormatPmHtml(Post postEntity)
        {
            var list = new List<Post> { postEntity };
            var thread = new Thread()
            {
                IsPrivateMessage = true
            };
            Html = await HtmlFormater.FormatThreadHtml(thread, list, GetTheme, true);
        }

        public void Reply()
        {
            
        }
    }
}
