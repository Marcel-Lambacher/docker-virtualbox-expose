namespace DockerVirtualBoxExpose.Common.Entities
{
    public class ExposedService
    {
        public string ContainerId { get; set; }

        public int Port { get; set; }

        public ExposedServiceState State { get; set; } 
    }
}