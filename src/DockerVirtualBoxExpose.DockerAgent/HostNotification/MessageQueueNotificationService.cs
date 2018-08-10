using System;
using DockerVirtualBoxExpose.Common.Entities;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;

namespace DockerVirtualBoxExpose.DockerAgent.Services
{
    public class MessageQueueNotificationService : IHostNotificationService, IDisposable
    {
        private readonly string _connectionString;
        private PushSocket _pushSocket;

        public MessageQueueNotificationService(string address, int port)
        {
            _connectionString = $"tcp://{address}:{port}";
        }

        public void Start()
        {
            _pushSocket?.Dispose();
            _pushSocket = new PushSocket(_connectionString);
        }

        public void Notify(ExposedService exposedService)
        {
            var frame = GetSerializedMessageFrame(exposedService);
            _pushSocket.SendFrame(frame);
        }

        public void Dispose()
        {
            _pushSocket.Dispose();
        }

        private string GetSerializedMessageFrame(ExposedService exposedService)
        {
            return JsonConvert.SerializeObject(exposedService);
        }
    }
}