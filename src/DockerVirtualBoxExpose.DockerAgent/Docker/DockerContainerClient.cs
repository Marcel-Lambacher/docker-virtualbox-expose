using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using DockerVirtualBoxExpose.Common.Entities;
using Serilog;

namespace DockerVirtualBoxExpose.DockerAgent.Docker
{
    public class DockerContainerClient: IDockerContainerClient, IDisposable
    {
        private readonly IDockerClient _dockerClient;

        public DockerContainerClient(IDockerClient dockerClient)
        {
            _dockerClient = dockerClient;
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
            Log.Logger.ForContext<DockerContainerClient>().Debug("Docker client has been disposed.");
        }
    }
}