namespace DockerVirtualBoxExpose.DockerAgent.Watchdog
{
    public interface IWatcher<in TEvent>
    {
        void WatchEventRaised(TEvent watchdogEvent);
    }
}