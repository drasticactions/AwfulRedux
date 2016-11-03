using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.System.RemoteSystems;
using Windows.UI.Core;
using AwfulForumsLibrary.Models.Web;
using AwfulRedux.Tools.Errors;
using AwfulRedux.UI.Models.Threads;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class ShareDevicesViewModel : ViewModelBase
    {

        private bool _isOpen = default(bool);

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                Set(ref _isOpen, value);
            }
        }

        public void Setup(Thread thread)
        {
            _thread = thread;
            Devices = new ObservableCollection<RemoteSystem>();
            deviceMap = new Dictionary<string, RemoteSystem>();
            BuildDeviceList();
        }

        private Thread _thread;
        private RemoteSystemWatcher remoteSystemWatcher;
        private Dictionary<string, RemoteSystem> deviceMap;

        public ObservableCollection<RemoteSystem> Devices { get; set; }

        private async void BuildDeviceList()
        {
            RemoteSystemAccessStatus accessStatus = await RemoteSystem.RequestAccessAsync();
            if (accessStatus == RemoteSystemAccessStatus.Allowed)
            {
                remoteSystemWatcher = RemoteSystem.CreateWatcher(MakeFilterList());

                // Subscribing to the event raised when a new remote system is found by the watcher.
                remoteSystemWatcher.RemoteSystemAdded += RemoteSystemWatcher_RemoteSystemAdded;

                // Subscribing to the event raised when a previously found remote system is no longer available.
                remoteSystemWatcher.RemoteSystemRemoved += RemoteSystemWatcher_RemoteSystemRemoved;

                remoteSystemWatcher.Start();
            }
        }

        private async void RemoteSystemWatcher_RemoteSystemRemoved(RemoteSystemWatcher sender, RemoteSystemRemovedEventArgs args)
        {
            if (deviceMap.ContainsKey(args.RemoteSystemId))
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => Devices.Remove(deviceMap[args.RemoteSystemId]));
                deviceMap.Remove(args.RemoteSystemId);
            }
        }

        private async void RemoteSystemWatcher_RemoteSystemAdded(RemoteSystemWatcher sender, RemoteSystemAddedEventArgs args)
        {
            if (!deviceMap.ContainsKey(args.RemoteSystem.Id))
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => Devices.Add(args.RemoteSystem));
                deviceMap[args.RemoteSystem.Id] = args.RemoteSystem;
            }
        }

        public async Task LaunchOnSystem(RemoteSystem system)
        {
            string test = "{" + string.Format("type:'toast', 'threadId':{0}, 'pageNumber':{1}, 'isThreadBookmark':{2}", _thread.ThreadId, _thread.CurrentPage, _thread.IsBookmark.ToString().ToLower()) + "}";
            RemoteLauncherOptions option = new RemoteLauncherOptions();
            option.FallbackUri = new Uri("https://www.microsoft.com/store/apps/9WZDNCRDSXMX");
            RemoteLaunchUriStatus launchUriStatus = await RemoteLauncher.LaunchUriAsync(new RemoteSystemConnectionRequest(system),
                new Uri($"awful:{test}"));
        }

        private List<IRemoteSystemFilter> MakeFilterList()
        {
            // construct an empty list
            List<IRemoteSystemFilter> localListOfFilters = new List<IRemoteSystemFilter>();

            // construct a discovery type filter that only allows "any" connections:
            RemoteSystemDiscoveryTypeFilter discoveryFilter = new RemoteSystemDiscoveryTypeFilter(RemoteSystemDiscoveryType.Any);

            // For this kind of filter, we must first create an IIterable of strings representing the device types to allow.
            // These strings are stored as static read-only properties of the RemoteSystemKinds class.
            List<String> listOfTypes = new List<String>
            {
                RemoteSystemKinds.Desktop,
                RemoteSystemKinds.Phone,
                RemoteSystemKinds.Holographic,
                RemoteSystemKinds.Xbox,
                RemoteSystemKinds.Hub
            };

            // Put the list of device types into the constructor of the filter
            RemoteSystemKindFilter kindFilter = new RemoteSystemKindFilter(listOfTypes);


            // construct an availibility status filter that only allows devices marked as available:
            RemoteSystemStatusTypeFilter statusFilter = new RemoteSystemStatusTypeFilter(RemoteSystemStatusType.Available);


            // add the 3 filters to the listL
            localListOfFilters.Add(discoveryFilter);
            localListOfFilters.Add(kindFilter);
            localListOfFilters.Add(statusFilter);

            // return the list
            return localListOfFilters;
        }
    }
}
