using Cc.Mvvm;

namespace Cc.Gui.Splash
{
    public class SplashViewModel : ViewModelBase
    {
        private string _appName;

        private string _vendor;

        private string _appVersion;

        private string _status;

        public string AppName
        {
            get { return _appName; }
            set
            {
                _appName = value;
                OnPropertyChanged();
            }
        }

        public string AppVersion
        {
            get { return _appVersion; }
            set
            {
                _appVersion = value;
                OnPropertyChanged();
            }
        }

        public string Vendor
        {
            get { return _vendor; }
            set
            {
                _vendor = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }
    }
}