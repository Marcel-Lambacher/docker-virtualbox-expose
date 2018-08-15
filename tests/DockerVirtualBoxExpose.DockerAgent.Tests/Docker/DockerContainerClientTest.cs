using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using DockerVirtualBoxExpose.DockerAgent.Docker;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DockerVirtualBoxExpose.DockerAgent.Tests.Docker
{
    public class DockerContainerClientTest
    {
        [Fact]
        public void ShouldDisposeClientWhenDisposeGetsCalled()
        {
            var client = Substitute.For<IDockerClient>();
            var containerClient = new DockerContainerClient(client);
            containerClient.Dispose();

            client.Received().Dispose();
        }

        [Fact]
        public async Task ShouldFilterForExposeLabelWhenQueryingContainers()
        {
            var client = Substitute.For<IDockerClient>();
            var containerClient = new DockerContainerClient(client);
            await containerClient.GetExposedServices();

            await client.Containers.Received().ListContainersAsync(Arg.Is<ContainersListParameters>(x =>
                x.Filters["label"][ExposedServiceLabelParser.ExposedServiceLabel]));
        }

        [Fact]
        public async Task ShouldReturnExposedServicesForAllContainersWhenQueryingContainers()
        {
            var client = Substitute.For<IDockerClient>();
            client.Containers
                .ListContainersAsync(Arg.Any<ContainersListParameters>())
                .Returns(Task.FromResult<IList<ContainerListResponse>>(new List<ContainerListResponse>
                {
                    new ContainerListResponse
                    {
                        ID = "test1",
                        Ports = new List<Port>
                        {
                            new Port { IP = "127.0.0.1", PrivatePort = 20, PublicPort = 80},
                            new Port { IP = "127.0.0.1", PrivatePort = 21, PublicPort = 81}
                        }
                    },
                    new ContainerListResponse
                    {
                        ID = "test2",
                        Labels = new Dictionary<string, string>
                        {
                            [ExposedServiceLabelParser.ExposedServiceLabel] = "22, 20"
                        }
                    }
                }));

            var containerClient = new DockerContainerClient(client);
            var exposedServices = (await containerClient.GetExposedServices()).ToList();

            exposedServices.Count.Should().Be(4);
            exposedServices[0].ContainerId.Should().Be("test1");
            exposedServices[0].Port.Should().Be(80);
            exposedServices[1].ContainerId.Should().Be("test1");
            exposedServices[1].Port.Should().Be(81);
            exposedServices[2].ContainerId.Should().Be("test2");
            exposedServices[2].Port.Should().Be(22);
            exposedServices[3].ContainerId.Should().Be("test2");
            exposedServices[3].Port.Should().Be(20);
        }
    }
}
