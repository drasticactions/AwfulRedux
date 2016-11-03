using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AwfulForumsLibrary.Tools;
using AwfulRedux.Controls;
using AwfulRedux.Tools.Helper;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class ThreadPageViewModel: ViewModelBase
    {
        public ThreadView ThreadView { get; set; }
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var toastArgs = parameter as ToastNotificationArgs;
            if (toastArgs != null)
            {
                var url = string.Format(EndPoints.ThreadPage, toastArgs.threadId);
                var newThreadEntity = new Thread()
                {
                    Location = url
                };
                if (toastArgs.pageNumber > 0)
                {
                    newThreadEntity.CurrentPage = toastArgs.pageNumber;
                }
                ThreadView.ViewModel.Selected = newThreadEntity;
                await ThreadView.ViewModel.LoadThread();
                return;
            }
            string parameterPassed = parameter as string;
            if (string.IsNullOrEmpty(parameterPassed)) return;
            var thread = JsonConvert.DeserializeObject<Thread>(parameterPassed);
            ThreadView.ViewModel.Selected = thread;
            await ThreadView.ViewModel.LoadThread();
        }
    }
}
