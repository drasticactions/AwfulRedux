using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Controls;
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
            string parameterPassed = parameter as string;
            if (string.IsNullOrEmpty(parameterPassed)) return;
            var thread = JsonConvert.DeserializeObject<Thread>(parameterPassed);
            ThreadView.ViewModel.Selected = thread;
            await ThreadView.ViewModel.LoadThread();
        }
    }
}
