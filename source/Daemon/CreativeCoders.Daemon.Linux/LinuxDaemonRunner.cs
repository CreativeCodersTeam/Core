using System;
using System.Runtime.Loader;
using System.Threading;
using CreativeCoders.Daemon.Base;
using CreativeCoders.Daemon.Runner.Base;

namespace CreativeCoders.Daemon.Linux
{
    public class LinuxDaemonRunner<TDaemon> : DaemonRunnerBase<TDaemon> where TDaemon : class, IDaemon, new()
    {
        private readonly TDaemon _daemon;

        private bool _daemonStopped;

        public LinuxDaemonRunner()
        {
            _daemon = new TDaemon();
        }

        public override void Run()
        {
            Console.WriteLine("Starting daemon...");

            AssemblyLoadContext.Default.Unloading += SigTermEventHandler;
            Console.CancelKeyPress += SigIntEventHandler;

            _daemon.StateChanged += (sender, state) => _daemonStopped = state == DaemonState.Stopped;
            _daemon.OnStart();

            while (!_daemonStopped)
            {
                Thread.Sleep(1000);
            }
        }

        private void SigIntEventHandler(object sender, ConsoleCancelEventArgs e)
        {
            StopDaemon();
        }

        private void SigTermEventHandler(AssemblyLoadContext obj)
        {
            StopDaemon();
        }

        private void StopDaemon()
        {
            _daemon.OnStop();
        }
    }
}