using System;
using DockerVirtualBoxExpose.DockerAgent.Services;
using DockerVirtualBoxExpose.DockerAgent.Watchdog;

namespace DockerVirtualBoxExpose.DockerAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var notificationService = new MessageQueueNotificationService("localhost", 5556);

            var exposedServiceWatcher = new ExposedServiceWatcher(notificationService);

            var watchdog = new DockerWatchdog();
            watchdog.AssignWatcher(exposedServiceWatcher);

            watchdog.Start();

            Console.Read();
        }
    }
}