using System;
using DockerVirtualBoxExpose.Common.Entities;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using Serilog;

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
            Log.Logger.ForContext<MessageQueueNotificationService>().Information("New container event: {@ExposedService}", exposedService);
            var frame = GetSerializedMessageFrame(exposedService);

            try
            {
                _pushSocket.SendFrame(frame);
            }
            catch (Exception exception)
            {
                Log.Logger.ForContext<MessageQueueNotificationService>().Error(exception, "Error while sending exposed service change to the message queue.");
            }
        }

        public void Dispose()
        {
            _pushSocket?.Dispose();
            Log.Logger.ForContext<MessageQueueNotificationService>().Information("Message queue server has been disposed.");
        }

        private string GetSerializedMessageFrame(ExposedService exposedService)
        {
            return JsonConvert.SerializeObject(exposedService);
        }
    }
}