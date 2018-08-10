using System;
using DockerVirtualBoxExpose.Common.Entities;

namespace DockerVirtualBoxExpose.DockerAgent.Watchdog
{
    public class DockerWatchdog: PollingService
    {
        private const int PollingIntervalMilliseconds = 1000;
        private IWatcher<ExposedService> _watcher;

        public DockerWatchdog(): base(PollingIntervalMilliseconds) { }

        public void AssignWatcher(IWatcher<ExposedService> watcher)
        {
            _watcher = watcher;
        }

        protected override void Poll()
        {
            Console.WriteLine("event triggered...");
        }
    }
}
