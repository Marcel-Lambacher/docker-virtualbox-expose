using DockerVirtualBoxExpose.Common.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DockerVirtualBoxExpose.DockerAgent.Watchdog
{
    public class ExposedServiceHistory
    {
        private List<ExposedService> _currentHistory = new List<ExposedService>();
        private List<ExposedService> _newHistory = new List<ExposedService>();

        public void Update(List<ExposedService> exposedServices)
        {
            _newHistory = exposedServices;
        }

        public IEnumerable<ExposedService> GetAddedServices()
        {
            return _newHistory.Except(_currentHistory);
        }

        public IEnumerable<ExposedService> GetRemovedServices()
        {
            return _currentHistory.Except(_newHistory);
        }

        public void Commit()
        {
            _currentHistory = _newHistory;
        }
    }
}