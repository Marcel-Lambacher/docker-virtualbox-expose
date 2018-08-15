using System;

namespace DockerVirtualBoxExpose.Common.Entities
{
    public class ExposedService: IEquatable<ExposedService>
    {
        public string ContainerId { get; }

        public int Port { get; }

        public ExposedServiceState State { get; set; }

        public ExposedService(string containerId, int port)
        {
            ContainerId = containerId;
            Port = port;
        }

        public bool Equals(ExposedService other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(ContainerId, other.ContainerId) && Port == other.Port;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((ExposedService) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ContainerId != null ? ContainerId.GetHashCode() : 0) * 397) ^ Port;
            }
        }
    }
}