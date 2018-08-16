using System;
using Docker.DotNet;
using DockerVirtualBoxExpose.Common.Entities;
using DockerVirtualBoxExpose.DockerAgent.Docker;
using DockerVirtualBoxExpose.DockerAgent.HostNotification;
using DockerVirtualBoxExpose.DockerAgent.Watchdog;
using NetMQ.Sockets;

namespace DockerVirtualBoxExpose.DockerAgent
{
    public sealed class DockerAgentService: DockerService
    {
        private readonly DockerWatchdog _dockerWatchdog;
        private readonly IWatcher<ExposedService> _exposedServiceWatcher;

        public DockerAgentService(DockerWatchdog watchdog, IWatcher<ExposedService> exposedServiceWatcher) : base(null)
        {
            _exposedServiceWatcher = exposedServiceWatcher;
            _dockerWatchdog = watchdog;
        }

        protected override void ServiceMain()
        {
            _dockerWatchdog.AssignWatcher(_exposedServiceWatcher);
            _dockerWatchdog.Start();
        }

        public override void Dispose()
        {
            _dockerWatchdog?.Dispose();
        }
    }
}
