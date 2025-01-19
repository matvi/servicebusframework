using Azure.Messaging.ServiceBus;

namespace Core.Interfaces;
public interface IServiceBusManager
{
    void AddConsumer<TConsumer>(string topicName, string suscrptionName, ServiceBusClient serviceBusClient, IServiceProvider serviceProvider) where TConsumer : IServiceBusConsumer;
    Task StartProcessingAsync(CancellationToken cancellationToken);
    Task StopProcessingAsync();
}