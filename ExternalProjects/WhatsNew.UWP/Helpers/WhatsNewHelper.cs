using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsNew.UWP.Helpers
{
    public enum WhatsNewState
    {
        Inactive = 0,
        Active,
        VersionUpdate
    }

    public class WhatsNewHelper : INotifyPropertyChanged
    {
        private const string LastVersionNumberKey = "WHATS_NEW_LAST_VERSION_NUMBER";

        private WhatsNewState state;
        private string lastVersionNumber = "1.0.0.0";
        public event PropertyChangedEventHandler PropertyChanged;
        public static readonly WhatsNewHelper Default = new WhatsNewHelper();

        public WhatsNewState State
        {
            get { return state; }
            internal set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }

        public string LastVersionNumber
        {
            get { return lastVersionNumber; }
            internal set
            {
                lastVersionNumber = value;
                OnPropertyChanged("LastVersionNumber");
            }
        }

        private WhatsNewHelper()
        {
            State = WhatsNewState.Active;
        }

        public void Launching()
        {
            if (State == WhatsNewState.Active)
            {
                LoadState();
            }
        }

        public string GetAppVersion()
        {
            var ver = Windows.ApplicationModel.Package.Current.Id.Version;
            return $"{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";
        }

        private void LoadState()
        {
            try
            {
                LastVersionNumber = StorageHelper.GetSetting<string>(LastVersionNumberKey);

                if (lastVersionNumber != GetAppVersion())
                {
                    lastVersionNumber = GetAppVersion();
                    State = WhatsNewState.VersionUpdate;
                }

                StoreState();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("FeedbackHelper.LoadState - Failed to load state, Exception: {0}", ex.ToString()));
            }
        }

        private void StoreState()
        {
            try
            {
                StorageHelper.StoreSetting(LastVersionNumberKey, LastVersionNumber, true);
                StorageHelper.FlushToStorage();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("FeedbackHelper.StoreState - Failed to store state, Exception: {0}", ex.ToString()));
            }

        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
