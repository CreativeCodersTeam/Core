using System;
using System.Threading;
using Cc.Core;

namespace Cc.Gui.Splash
{
    public class SplashScreen : ISplashScreen
    {
        private SplashWindow _splashWindow;

        private readonly object _splashSyncLock = new object();

        private bool _closing;

        public void Show(SplashViewModel splashViewModel)
        {
            Assert.IsNotNull(splashViewModel, "splashViewModel");

            var splashThread = new Thread(() => ShowSplash(splashViewModel));
            splashThread.SetApartmentState(ApartmentState.STA);
            splashThread.Start();
        }

        public void Close()
        {
            lock (_splashSyncLock)
            {
                if (_splashWindow != null)
                {
                    _splashWindow.Dispatcher.BeginInvoke(new Action(() => _splashWindow.Close()));
                }
                else
                {
                    _closing = true;
                }
            }
        }

        private void ShowSplash(SplashViewModel splashViewModel)
        {
            lock (_splashSyncLock)
            {
                if (_closing)
                {
                    _closing = false;
                    return;
                }
                _splashWindow = new SplashWindow(splashViewModel);
            }
            _splashWindow.ShowDialog();
        }
    }
}