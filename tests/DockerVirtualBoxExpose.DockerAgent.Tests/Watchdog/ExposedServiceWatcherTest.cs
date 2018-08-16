using DockerVirtualBoxExpose.Common.Entities;
using DockerVirtualBoxExpose.DockerAgent.HostNotification;
using DockerVirtualBoxExpose.DockerAgent.Watchdog;
using NSubstitute;
using Xunit;

namespace DockerVirtualBoxExpose.DockerAgent.Tests.Watchdog
{
    public class ExposedServiceWatcherTest
    {
        [Fact]
        public void ShouldNotifyWhenCallingWatchEventRaised()
        {
            var notificationService = Substitute.For<IHostNotificationService>();
            var watcher = new ExposedServiceWatcher(notificationService);

            var exposedService = new ExposedService("test", 80);

            watcher.WatchEventRaised(exposedService);
            notificationService.Received().Notify(exposedService);
        }
    }
}
