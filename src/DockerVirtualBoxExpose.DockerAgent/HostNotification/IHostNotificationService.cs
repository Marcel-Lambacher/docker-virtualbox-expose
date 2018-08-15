using DockerVirtualBoxExpose.Common.Entities;

namespace DockerVirtualBoxExpose.DockerAgent.HostNotification
{
    public interface IHostNotificationService
    {
        void Notify(ExposedService exposedService);
    }
}