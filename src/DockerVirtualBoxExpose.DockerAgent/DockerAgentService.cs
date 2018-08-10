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
        }

        public override void Dispose()
        {
            _dockerWatchdog?.Dispose();
            _notificationService?.Dispose();
        }
    }
}
