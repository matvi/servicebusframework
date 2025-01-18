namespace Core.Interfaces;
public interface IServiceBusManager
{
    void AddConsumer<TConsumer>(string topicName, string suscrptionName) where TConsumer : IServiceBusConsumer;
    Task StartProcessingAsync(CancellationToken cancellationToken);
    Task StopProcessingAsync();
}