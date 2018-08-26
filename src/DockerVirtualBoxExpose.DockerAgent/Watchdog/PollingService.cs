using System;
using System.Threading.Tasks;
using System.Timers;
using Serilog;

namespace DockerVirtualBoxExpose.DockerAgent.Watchdog
{
    public abstract class PollingService: IDisposable
    {
        private readonly Timer _timer;

        protected PollingService(int pollingInterval)
        {
            _timer = new Timer(pollingInterval);
            _timer.Elapsed += TimerOnElapsed;
        }

        public void Start()
        {
            Log.Logger.ForContext<PollingService>().Information("A polling service has been started.");
            _timer.Start();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _timer.Elapsed -= TimerOnElapsed;
            _timer?.Dispose();

            Log.Logger.ForContext<PollingService>().Information("A polling service has been disposed.");
        }

        private async void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
           await Poll();
        }

        protected abstract Task Poll();
    }
}
