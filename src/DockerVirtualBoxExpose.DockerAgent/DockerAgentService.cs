using DockerVirtualBoxExpose.Common.Entities;
using DockerVirtualBoxExpose.DockerAgent.Docker;
using DockerVirtualBoxExpose.DockerAgent.Watchdog;
using Serilog;

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

            Log.Logger.ForContext<DockerAgentService>().Information("Starting docker watchdog...");
            _dockerWatchdog.Start();
        }

        public override void Dispose()
        {
            _dockerWatchdog?.Dispose();
            Log.Logger.ForContext<DockerAgentService>().Debug("The docker agent service has been disposed.");
        }
    }
}
