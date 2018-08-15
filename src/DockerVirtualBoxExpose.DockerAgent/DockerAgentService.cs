using DockerVirtualBoxExpose.DockerAgent.Docker;
using DockerVirtualBoxExpose.DockerAgent.HostNotification;
using DockerVirtualBoxExpose.DockerAgent.Watchdog;

namespace DockerVirtualBoxExpose.DockerAgent
{
    public sealed class DockerAgentService: DockerService
    {
        private MessageQueueNotificationService _notificationService;
        private DockerContainerClient _dockerContainerClient;
        private DockerWatchdog _dockerWatchdog;

        public DockerAgentService(string[] args) : base(args)  { }

        protected override void ServiceMain()
        {
            _notificationService = new MessageQueueNotificationService("localhost", 5556);

            var exposedServiceWatcher = new ExposedServiceWatcher(_notificationService);

            _dockerContainerClient = new DockerContainerClient("unix:///var/run/docker.sock");
            _dockerWatchdog = new DockerWatchdog(_dockerContainerClient);
            _dockerWatchdog.AssignWatcher(exposedServiceWatcher);

            _dockerWatchdog.Start();
        }

        public override void Dispose()
        {
            _dockerWatchdog?.Dispose();
            _dockerContainerClient?.Dispose();
            _notificationService?.Dispose();
        }
    }
}
