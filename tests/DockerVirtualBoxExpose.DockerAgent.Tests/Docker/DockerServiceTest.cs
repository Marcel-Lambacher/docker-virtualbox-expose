using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DockerVirtualBoxExpose.DockerAgent.Docker;
using NSubstitute;
using Xunit;

namespace DockerVirtualBoxExpose.DockerAgent.Tests.Docker
{
    public class DockerServiceMock : DockerService
    {
        public DockerServiceMock() : base(null) {  }

        public override void Dispose()
        { }

        protected override void ServiceMain()
        {
            PublicServiceMain();
        }

        public void PublicServiceMain() { }
    }

    public class DockerServiceTest
    {
        [Fact]
        public void ShouldDisposeWhenExitGetsCalled()
        {
            var service = Substitute.For<DockerServiceMock>();

            using (Task.Run(() => service.Start()))
            {
                service.Exit();
                service.Received().Dispose();
            }
        }

        [Fact]
        public void ShouldEnterServiceMainWhenStartGetsCalled()
        {
            var service = Substitute.For<DockerServiceMock>();

            using (Task.Run(() => service.Start()))
            {
                service.Exit();
                service.Received().PublicServiceMain();
            }
        }
    }
}
