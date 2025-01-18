using Azure.Messaging.ServiceBus;

namespace Core.Interfaces
{
    public interface IServiceBusConsumer
    {
        Task ProcessMessage(ProcessMessageEventArgs args);
        Task ProcessError(ProcessErrorEventArgs args);
    }
}
