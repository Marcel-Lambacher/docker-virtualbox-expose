using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DockerVirtualBoxExpose.Common.Entities;

namespace DockerVirtualBoxExpose.DockerAgent.Docker
{
    public interface IDockerContainerClient
    {
        Task<IEnumerable<ExposedService>> GetExposedServices();
    }
}
