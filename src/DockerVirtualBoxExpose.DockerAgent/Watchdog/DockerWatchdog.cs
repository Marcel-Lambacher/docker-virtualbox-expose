using System;
using System.Threading.Tasks;
using DockerVirtualBoxExpose.Common.Entities;
using DockerVirtualBoxExpose.DockerAgent.Docker;

namespace DockerVirtualBoxExpose.DockerAgent.Watchdog
{
    public sealed class DockerWatchdog: PollingService
    {
        private readonly IDockerContainerClient _dockerClient;
        private const int PollingIntervalMilliseconds = 1000;
        private IWatcher<ExposedService> _watcher;

        public DockerWatchdog(IDockerContainerClient dockerClient): base(PollingIntervalMilliseconds)
        {
            _dockerClient = dockerClient;
        }

        public void AssignWatcher(IWatcher<ExposedService> watcher)
        {
            _watcher = watcher;
        }

        protected override async Task Poll()
        {
            //TODO: Diff between previous history
            var exposedServices = await _dockerClient.GetExposedServices();
            foreach (var exposedService in exposedServices)
            {
                _watcher.WatchEventRaised(exposedService);
            }
        }
    }
}
