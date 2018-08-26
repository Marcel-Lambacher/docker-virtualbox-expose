using System;
using Docker.DotNet;
using DockerVirtualBoxExpose.Common.Entities;
using DockerVirtualBoxExpose.DockerAgent.Docker;
using DockerVirtualBoxExpose.DockerAgent.HostNotification;
using DockerVirtualBoxExpose.DockerAgent.Watchdog;
using Microsoft.Extensions.DependencyInjection;
using NetMQ.Sockets;
using Serilog;
using Serilog.Events;

namespace DockerVirtualBoxExpose.DockerAgent
{
    internal class Program
    {
        private const string MessageQueueUri = "tcp://localhost:5556";
        private const string DockerSocketUri = "unix:///var/run/docker.sock";
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
                    new DockerClientConfiguration(new Uri(DockerSocketUri)).CreateClient())
                .AddTransient<IDockerContainerClient, DockerContainerClient>()
                .AddTransient<DockerWatchdog>()
                .AddTransient<DockerAgentService>();

            Log.Logger.ForContext<Program>().Information("Message queue server is listening on {uri}", MessageQueueUri);
            Log.Logger.ForContext<Program>().Information("Docker client is listening on {uri}", DockerSocketUri);

            using (var serviceProvider = collection.BuildServiceProvider())
            {
                var service = serviceProvider.GetService<DockerAgentService>();
                Console.CancelKeyPress += (sender, eventArgs) => service.Exit();
                service.Start();
            }
            
            Log.Logger.ForContext<Program>().Information("Docker agent service has been terminated successfully.");
        }
    }
}