using System.Collections.Generic;
using System.Linq;
using Docker.DotNet.Models;
using DockerVirtualBoxExpose.DockerAgent.Docker;
using FluentAssertions;
using Xunit;

namespace DockerVirtualBoxExpose.DockerAgent.Tests.Docker
{
    public class ExposedServiceLabelParserTest
    {
        [Fact]
        public void ShouldReturnExposedPortsWhenLabelIsNotGiven()
        {
            var response = new ContainerListResponse
            {
                ID = "test",
                Ports = new List<Port>
                {
                    new Port { IP = "127.0.0.1", PrivatePort = 20, PublicPort = 80},
                    new Port { IP = "127.0.0.1", PrivatePort = 21, PublicPort = 81}
                }
            };

            var exposedServices = ExposedServiceLabelParser.GetExposedServicesFromContainer(response).ToList();

            exposedServices.Count.Should().Be(2);
            exposedServices[0].ContainerId.Should().Be("test");
            exposedServices[0].Port.Should().Be(80);
            exposedServices[1].ContainerId.Should().Be("test");
            exposedServices[1].Port.Should().Be(81);
        }

        [Fact]
        public void ShouldReturnExposedPortsWhenLabelHasNoValue()
        {
            var response = new ContainerListResponse
            {
                ID = "test",
                Labels = new Dictionary<string, string>
                {
                    [ExposedServiceLabelParser.ExposedServiceLabel] = null
                },
                Ports = new List<Port>
                {
                    new Port { IP = "127.0.0.1", PrivatePort = 20, PublicPort = 80},
                    new Port { IP = "127.0.0.1", PrivatePort = 21, PublicPort = 81}
                }
            };

            var exposedServices = ExposedServiceLabelParser.GetExposedServicesFromContainer(response).ToList();

            exposedServices.Count.Should().Be(2);
            exposedServices[0].ContainerId.Should().Be("test");
            exposedServices[0].Port.Should().Be(80);
            exposedServices[1].ContainerId.Should().Be("test");
            exposedServices[1].Port.Should().Be(81);
        }

        [Fact]
        public void ShouldReturnLabelPortsWhenLabelHasValue()
        {
            var response = new ContainerListResponse
            {
                ID = "test",
                Labels = new Dictionary<string, string>
                {
                    [ExposedServiceLabelParser.ExposedServiceLabel] = "22"
                },
                Ports = new List<Port>
                {
                    new Port { IP = "127.0.0.1", PrivatePort = 20, PublicPort = 80},
                    new Port { IP = "127.0.0.1", PrivatePort = 21, PublicPort = 81}
                }
            };

            var exposedServices = ExposedServiceLabelParser.GetExposedServicesFromContainer(response).ToList();

            exposedServices.Count.Should().Be(1);
            exposedServices[0].ContainerId.Should().Be("test");
            exposedServices[0].Port.Should().Be(22);
        }

        [Fact]
        public void ShouldReturnLabelPortsWhenLabelHasMultipleValues()
        {
            var response = new ContainerListResponse
            {
                ID = "test",
                Labels = new Dictionary<string, string>
                {
                    [ExposedServiceLabelParser.ExposedServiceLabel] = "22, 20 , 30"
                },
                Ports = new List<Port>
                {
                    new Port { IP = "127.0.0.1", PrivatePort = 20, PublicPort = 80},
                    new Port { IP = "127.0.0.1", PrivatePort = 21, PublicPort = 81}
                }
            };

            var exposedServices = ExposedServiceLabelParser.GetExposedServicesFromContainer(response).ToList();

            exposedServices.Count.Should().Be(3);
            exposedServices[0].ContainerId.Should().Be("test");
            exposedServices[0].Port.Should().Be(22);
            exposedServices[1].ContainerId.Should().Be("test");
            exposedServices[1].Port.Should().Be(20);
            exposedServices[2].ContainerId.Should().Be("test");
            exposedServices[2].Port.Should().Be(30);
        }

        [Fact]
        public void ShouldNotThrowExceptionWhenInvalidPortLabelIsGiven()
        {
            var response = new ContainerListResponse
            {
                ID = "test",
                Labels = new Dictionary<string, string>
                {
                    [ExposedServiceLabelParser.ExposedServiceLabel] = "80, invalid, 443"
                }
            };

            var exposedServices = ExposedServiceLabelParser.GetExposedServicesFromContainer(response).ToList();

            exposedServices.Count.Should().Be(2);
            exposedServices[0].ContainerId.Should().Be("test");
            exposedServices[0].Port.Should().Be(80);
            exposedServices[1].ContainerId.Should().Be("test");
            exposedServices[1].Port.Should().Be(443);
        }
    }
}
