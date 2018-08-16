using System;
using System.ServiceModel.Channels;
using System.Text;
using DockerVirtualBoxExpose.Common.Entities;
using DockerVirtualBoxExpose.DockerAgent.HostNotification;
using FluentAssertions;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace DockerVirtualBoxExpose.DockerAgent.Tests.HostNotification
{
    public class MessageQueueNotificationServiceTest
    {
        [Fact]
        public void ShouldDisposePushSocketWhenCallingDispose()
        {
            var socket = Substitute.For<PushSocket>(string.Empty);
            var service = new MessageQueueNotificationService(socket);
            service.Dispose();

            socket.Received().Dispose();
        }

        [Fact]
        public void ShouldSendFrameWhenCallingNotify()
        {
            var socket = Substitute.For<PushSocket>(string.Empty);
            var service = new MessageQueueNotificationService(socket);

            var message = new Msg();
            string response = null;
            socket.WhenForAnyArgs(x => x.TrySend(ref message, Arg.Any<TimeSpan>(), Arg.Any<bool>()))
                .Do(x =>
                {
                    response = Encoding.UTF8.GetString(x.Arg<Msg>().Data);
                });

            var exposedService = new ExposedService("test", 80) {State = ExposedServiceState.ServiceAdded};
            service.Notify(exposedService);

            response.Should().Be(JsonConvert.SerializeObject(exposedService));
        }
    }
}
