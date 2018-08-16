using System;
using Docker.DotNet;
using DockerVirtualBoxExpose.Common.Entities;
using DockerVirtualBoxExpose.DockerAgent.Docker;
using DockerVirtualBoxExpose.DockerAgent.HostNotification;
using DockerVirtualBoxExpose.DockerAgent.Watchdog;
using Microsoft.Extensions.DependencyInjection;
using NetMQ.Sockets;

namespace DockerVirtualBoxExpose.DockerAgent
{
    internal class Program
    {
        static void Main()
        {
            var collection = new ServiceCollection()
                .AddSingleton<IHostNotificationService>(x =>
                    new MessageQueueNotificationService(new PushSocket("tcp://localhost:5556")))
                .AddTransient<IWatcher<ExposedService>, ExposedServiceWatcher>()
                .AddSingleton<IDockerClient>(x =>
                    new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient())
                .AddTransient<IDockerContainerClient, DockerContainerClient>()
                .AddTransient<DockerWatchdog>()
                .AddTransient<DockerAgentService>();

            using (var serviceProvider = collection.BuildServiceProvider())
            {
                var service = serviceProvider.GetService<DockerAgentService>();

                Console.CancelKeyPress += (sender, eventArgs) => service.Exit();

                service.Start();
            }
        }
    }
}