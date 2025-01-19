using Azure.Messaging.ServiceBus;
using Core.Interfaces;


namespace Application.ServiceBusConsumers
{
    // This message comes from TMSystem when it creates a shipment in their system
    public class TestConsumer : IServiceBusConsumer
    {
        private readonly ILogger<TestConsumer> _logger;

        public TestConsumer(ILogger<TestConsumer> logger)
        {
            _logger = logger;
        }
        public Task ProcessError(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Error processing message: {ErrorSource}", args.ErrorSource);
            return Task.CompletedTask;
        }

        public async Task ProcessMessage(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            _logger.LogInformation("TestConsumer received message: {MessageBody}", body);
            await args.CompleteMessageAsync(args.Message);
        }
    }
}
