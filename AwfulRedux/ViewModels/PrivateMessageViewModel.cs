using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using AwfulForumsLibrary.Managers;
using AwfulForumsLibrary.Models.Messages;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.Views;
using AwfulWebTemplate;
using Newtonsoft.Json;
using Template10.Mvvm;
using PrivateMessage = AwfulRedux.UI.Models.Messages.PrivateMessage;

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
            var threadTemplateModel = new PrivateMessageTemplateModel
            {
                PMPost = postEntity,
                IsDarkThemeSet = this.GetTheme == PlatformIdentifier.WindowsPhone
            };
            var threadTemplate = new PrivateMessageTemplate { Model = threadTemplateModel };
            Html = threadTemplate.GenerateString();
        }

        public void Reply()
        {
            Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(NewPrivateMessagePage),
                JsonConvert.SerializeObject(Selected));
        }
    }
}
