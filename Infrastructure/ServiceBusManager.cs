using Azure.Messaging.ServiceBus;
using Core.Interfaces;
using System.Collections.Concurrent;

namespace Infrastructure.Services
{
    public class ServiceBusManager : IServiceBusManager
    {
        private static readonly Lazy<ServiceBusManager> _instance = new Lazy<ServiceBusManager>(() => new ServiceBusManager());
        private readonly ConcurrentDictionary<string, ServiceBusProcessor> _processors;

        public ServiceBusManager()
        {
            _processors = new ConcurrentDictionary<string, ServiceBusProcessor>();
        }

        public static ServiceBusManager Instance => _instance.Value;

        public void AddConsumer<TConsumer>(string topicName, string subscriptionName, ServiceBusClient serviceBusClient, IServiceProvider serviceProvider) where TConsumer : IServiceBusConsumer
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentException("Topic name cannot be null or empty.", nameof(topicName));

            if (string.IsNullOrWhiteSpace(subscriptionName))
                throw new ArgumentException("Subscription name cannot be null or empty.", nameof(subscriptionName));

            var processor = serviceBusClient.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1,       
                PrefetchCount = 10            
            });

            processor.ProcessMessageAsync += async args =>
            {
                using var scope = serviceProvider.CreateAsyncScope();
                var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
                await consumer.ProcessMessage(args);
            };

            processor.ProcessErrorAsync += args =>
            {
                using var scope = serviceProvider.CreateAsyncScope();
                var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
                return consumer.ProcessError(args);
            };

            if (!_processors.TryAdd($"{topicName}:{subscriptionName}", processor))
            {
                throw new InvalidOperationException($"Consumer for {topicName}:{subscriptionName} is already registered.");
            }
        }

        public async Task StartProcessingAsync(CancellationToken cancellationToken)
        {
            var startTasks = _processors.Values.Select(processor => processor.StartProcessingAsync(cancellationToken));
            await Task.WhenAll(startTasks);
        }

        public async Task StopProcessingAsync()
        {
            var stopTasks = _processors.Values.Select(processor => processor.StopProcessingAsync());
            await Task.WhenAll(stopTasks);
        }
    }
}
