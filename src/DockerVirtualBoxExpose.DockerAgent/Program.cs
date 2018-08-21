using System;
using Docker.DotNet;
using DockerVirtualBoxExpose.Common.Entities;
using DockerVirtualBoxExpose.DockerAgent.Docker;
using DockerVirtualBoxExpose.DockerAgent.HostNotification;
using DockerVirtualBoxExpose.DockerAgent.Watchdog;
using Microsoft.Extensions.DependencyInjection;
using NetMQ.Sockets;
using Serilog;

namespace DockerVirtualBoxExpose.DockerAgent
{
    internal class Program
    {
        private const string MessageQueueUri = "tcp://localhost:5556";

        static void Main()
        {
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            var collection = new ServiceCollection()
                .AddSingleton<IHostNotificationService>(x =>
                    new MessageQueueNotificationService(new PushSocket(MessageQueueUri)))
                .AddTransient<IWatcher<ExposedService>, ExposedServiceWatcher>()
                .AddSingleton<IDockerClient>(x =>
                    new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient())
                .AddTransient<IDockerContainerClient, DockerContainerClient>()
                .AddTransient<DockerWatchdog>()
                .AddTransient<DockerAgentService>();

            Log.Logger.Information("Message queue server is running and listens on {uri}", MessageQueueUri);

            using (var serviceProvider = collection.BuildServiceProvider())
            {
                var service = serviceProvider.GetService<DockerAgentService>();

                Console.CancelKeyPress += (sender, eventArgs) => service.Exit();

                service.Start();
            }
        }
    }
}