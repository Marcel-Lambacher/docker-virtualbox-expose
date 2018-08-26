using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DockerVirtualBoxExpose.Common.Entities;
using DockerVirtualBoxExpose.DockerAgent.Docker;
using Serilog;

namespace DockerVirtualBoxExpose.DockerAgent.Watchdog
{
    public sealed class DockerWatchdog: PollingService
    {
        private readonly IDockerContainerClient _dockerClient;
        private const int PollingIntervalMilliseconds = 1000;
        private IWatcher<ExposedService> _watcher;
        private readonly ExposedServiceHistory _history = new ExposedServiceHistory();
        private readonly object _historyLock = new object();

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
            List<ExposedService> exposedServices;

            try
            {
                exposedServices = (await _dockerClient.GetExposedServices()).ToList();
            }
            catch (Exception exception)
            {
                Log.Logger.ForContext<DockerWatchdog>().Error(exception, "Error while retrieving current docker containers.");
                return;
            }

            lock (_historyLock)
            {
                _history.Update(exposedServices);

                NotifyServices(_history.GetAddedServices(), ExposedServiceState.ServiceAdded);
                NotifyServices(_history.GetRemovedServices(), ExposedServiceState.ServiceRemoved);

                _history.Commit();
            }
        }

        private void NotifyServices(IEnumerable<ExposedService> services, ExposedServiceState state)
        {
            foreach (var exposedService in services)
            {
                exposedService.State = state;
                _watcher.WatchEventRaised(exposedService);
            }
        }
    }
}
