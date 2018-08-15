using System;
using DockerVirtualBoxExpose.Common.Entities;
using DockerVirtualBoxExpose.DockerAgent.HostNotification;

namespace DockerVirtualBoxExpose.DockerAgent.Watchdog
{
    public class ExposedServiceWatcher: IWatcher<ExposedService>
    {
        private readonly IHostNotificationService _notificationService;

        public ExposedServiceWatcher(IHostNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void WatchEventRaised(ExposedService watchdogEvent)
        {
            _notificationService.Notify(watchdogEvent);
        }
    }
}