using System;
using DockerVirtualBoxExpose.Common.Entities;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;

namespace DockerVirtualBoxExpose.DockerAgent.HostNotification
{
    public sealed class MessageQueueNotificationService : IHostNotificationService, IDisposable
    {
        private readonly PushSocket _pushSocket;

        public MessageQueueNotificationService(PushSocket pushSocket)
        {
            _pushSocket = pushSocket;
        }

        public void Notify(ExposedService exposedService)
        {
            var frame = GetSerializedMessageFrame(exposedService);
            _pushSocket.SendFrame(frame);
        }

        public void Dispose()
        {
            _pushSocket?.Dispose();
        }

        private string GetSerializedMessageFrame(ExposedService exposedService)
        {
            return JsonConvert.SerializeObject(exposedService);
        }
    }
}