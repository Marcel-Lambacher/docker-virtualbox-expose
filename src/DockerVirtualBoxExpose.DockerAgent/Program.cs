using System;

namespace DockerVirtualBoxExpose.DockerAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new DockerAgentService(args);

            Console.CancelKeyPress += (sender, eventArgs) => service.Exit();

            service.Start();
        }
    }
}