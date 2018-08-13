using System;
using Docker.DotNet;
using Docker.DotNet.Models;
using DockerVirtualBoxExpose.DockerAgent.Docker;
using DockerVirtualBoxExpose.DockerAgent.Services;
using DockerVirtualBoxExpose.DockerAgent.Watchdog;

namespace DockerVirtualBoxExpose.DockerAgent
{
    public sealed class DockerAgentService: DockerService
    {
        private MessageQueueNotificationService _notificationService;
        private DockerWatchdog _dockerWatchdog;

        public DockerAgentService(string[] args) : base(args)  { }

        protected override void ServiceMain()
        {
            _notificationService = new MessageQueueNotificationService("localhost", 5556);

            var exposedServiceWatcher = new ExposedServiceWatcher(_notificationService);

            _dockerWatchdog = new DockerWatchdog();
            _dockerWatchdog.AssignWatcher(exposedServiceWatcher);

            _dockerWatchdog.Start();

            var client = new DockerClientConfiguration(new Uri("unix://var/run/docker.sock")).CreateClient();
            var containers = client.Containers.ListContainersAsync(new ContainersListParameters()).Result;

            foreach (var container in containers)
            {
                Console.WriteLine(string.Join(" - ", container.Names));
            }
        }

        public override void Dispose()
        {
            _dockerWatchdog?.Dispose();
            _notificationService?.Dispose();
        }
    }
}
