using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class PreviewViewModel : ViewModelBase
    {
        private bool _isLoading = default(bool);

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }

        private string _postHtml = default(string);

        public string PostHtml
        {
            get { return _postHtml; }
            set
            {
                Set(ref _postHtml, value);
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

        public async void LoadPost(Thread thread, Post post)
        {
            PostHtml = await HtmlFormater.FormatPreviewHtml(thread, post, GetTheme);
        }
    }
}
