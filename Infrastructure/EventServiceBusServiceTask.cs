
using Core.Interfaces;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EventServiceBusServiceTask : IEventServiceBusServiceTask
    {
        private readonly IServiceBusManager _serviceBusManager;

        public EventServiceBusServiceTask()
        {
            _serviceBusManager = ServiceBusManager.Instance;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {

            await _serviceBusManager.StartProcessingAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _serviceBusManager.StopProcessingAsync();
        }


    }
}
