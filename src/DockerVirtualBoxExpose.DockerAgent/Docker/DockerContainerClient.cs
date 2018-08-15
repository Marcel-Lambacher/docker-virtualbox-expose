using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using DockerVirtualBoxExpose.Common.Entities;

namespace DockerVirtualBoxExpose.DockerAgent.Docker
{
    public class DockerContainerClient: IDockerContainerClient, IDisposable
    {
        private readonly IDockerClient _dockerClient;

        public DockerContainerClient(string connectionString)
        {
            _dockerClient = new DockerClientConfiguration(new Uri(connectionString)).CreateClient();   
        }

        public async Task<IEnumerable<ExposedService>> GetExposedServices()
        {
            var searchParameters = new ContainersListParameters { All = false, Filters = new Dictionary<string, IDictionary<string, bool>>
                {
                    ["label"] = new Dictionary<string, bool>
                    {
                        [ExposedServiceLabelParser.ExposedServiceLabel] = true
                    }
                }};

            var containers = await _dockerClient.Containers.ListContainersAsync(searchParameters);
            return containers.SelectMany(ExposedServiceLabelParser.GetExposedServicesFromContainer);
        }       

        public void Dispose()
        {
            _dockerClient?.Dispose();
        }
    }
}