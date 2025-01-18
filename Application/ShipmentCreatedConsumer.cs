using Azure.Messaging.ServiceBus;
using Core.Interfaces;


namespace Application.ServiceBusConsumers
{
    public class ShipmentCreatedConsumer : IServiceBusConsumer
    {
        public Task ProcessError(ProcessErrorEventArgs args)
        {
             return Task.CompletedTask;
        }

        public async Task ProcessMessage(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            await args.CompleteMessageAsync(args.Message);
        }
    }
}
