using DockerVirtualBoxExpose.Common.Entities;

namespace DockerVirtualBoxExpose.DockerAgent.Services
{
    public interface IHostNotificationService
    {
        void Notify(ExposedService exposedService);
    }
}