using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using DockerVirtualBoxExpose.Common.Entities;
using DockerVirtualBoxExpose.DockerAgent.Watchdog;
using FluentAssertions;
using Xunit;

namespace DockerVirtualBoxExpose.DockerAgent.Tests.Watchdog
{
    public class ExposedServiceHistoryTest
    {
        [Fact]
        public void ShouldNotOverwriteHistoryWhenCallingUpdate()
        {
            var initialHistory = new List<ExposedService> {new ExposedService("test", 80)};

            var history = new ExposedServiceHistory();
            history.Update(initialHistory);

            history.GetAddedServices().Count().Should().Be(1);
            history.GetRemovedServices().Count().Should().Be(0);
        }

        [Fact]
        public void ShouldOverwriteHistoryWhenCallingCommit()
        {
            var initialHistory = new List<ExposedService> { new ExposedService("test", 80) };

            var history = new ExposedServiceHistory();
            history.Update(initialHistory);
            history.Commit();

            history.GetAddedServices().Count().Should().Be(0);
            history.GetRemovedServices().Count().Should().Be(0);
        }

        [Fact]
        public void ShouldReturnAddedServicesWhenCallingGetAddedServices()
        {
            var initialHistory = new List<ExposedService> { new ExposedService("test", 80) };

            var history = new ExposedServiceHistory();
            history.Update(initialHistory);
            history.Commit();

            var newHistory = new List<ExposedService> { new ExposedService("test2", 22), new ExposedService("test", 80) };

            history.Update(newHistory);

            history.GetAddedServices().Count().Should().Be(1);
            history.GetAddedServices().First().Should().Be(new ExposedService("test2", 22) { State = ExposedServiceState.ServiceAdded });

            history.GetRemovedServices().Count().Should().Be(0);
        }

        [Fact]
        public void ShouldReturnRemovedServicesWhenCallingGetRemovedServices()
        {
            var initialHistory = new List<ExposedService> { new ExposedService("test2", 22), new ExposedService("test", 80) };

            var history = new ExposedServiceHistory();
            history.Update(initialHistory);
            history.Commit();

            var newHistory = new List<ExposedService> { new ExposedService("test", 80) };

            history.Update(newHistory);

            history.GetAddedServices().Count().Should().Be(0);

            history.GetRemovedServices().Count().Should().Be(1);
            history.GetRemovedServices().First().Should().Be(new ExposedService("test2", 22) { State = ExposedServiceState.ServiceRemoved });
        }
    }
}
