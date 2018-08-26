using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace DockerVirtualBoxExpose.DockerAgent.Docker
{
    public abstract class DockerService: IDisposable
    {
        protected static readonly AutoResetEvent WaitHandle = new AutoResetEvent(false);
        protected readonly string[] ApplicationArguments;

        protected DockerService(string[] args)
        {
            ApplicationArguments = args;
        }

        public void Start()
        {
            Task.Run(() => ServiceMain());
            WaitForCancel();
        }

        public abstract void Dispose();

        public void Exit()
        {
            Log.Logger.ForContext<DockerService>().Information("A docker service has received a cancel request. Trying to dispose all resources...");
            Dispose();
            WaitHandle.Set();
        }

        protected abstract void ServiceMain();

        private void WaitForCancel()
        {
            WaitHandle.WaitOne();
        }
    }
}
