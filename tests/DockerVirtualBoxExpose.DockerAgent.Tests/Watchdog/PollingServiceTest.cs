using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DockerVirtualBoxExpose.DockerAgent.Watchdog;
using FluentAssertions;
using Xunit;

namespace DockerVirtualBoxExpose.DockerAgent.Tests.Watchdog
{
    public class PollingServiceMock : PollingService
    {
        public PollingServiceMock(int pollingInterval) : base(pollingInterval)  { }
        public int PollingCount { get; set; }

        protected override Task Poll()
        {
            PollingCount++;
            return Task.FromResult(0);
        }
    }

    public class PollingServiceTest
    {
        [Fact]
        public void ShouldDisposeWhenCallingDipose()
        {
            var service = new PollingServiceMock(100);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            service.Start();
            Thread.Sleep(250);
            service.Dispose();

            stopwatch.Stop();
            Thread.Sleep(250);

            var expectedPollCount = stopwatch.ElapsedMilliseconds / 100;

            service.PollingCount.Should().Be((int)expectedPollCount);
        }
    }
}
