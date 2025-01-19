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

            //if you uncomment the next line the program will start processing the events without the necessity of executing the hostedService.
            //but...............
            // I dont think its a good idea because StartProcessingAsync() should be executed after all services are registerd in the Program.cs
            //The second reason I dont think its a good idea its because we are not awaiting (await processor.StartProcessingAsync) which it might run out of threats in heavy applications.

            //processor.StartProcessingAsync();

            if (!_processors.TryAdd($"{topicName}:{subscriptionName}", processor))
            {
                throw new InvalidOperationException($"Consumer for {topicName}:{subscriptionName} is already registered.");
            }
            Console.WriteLine("Consumer added");
        }

        public async Task StartProcessingAsync(CancellationToken cancellationToken)
        {
            //currently processors is empty because the singleton is not working.
            Console.WriteLine("Starting processing ***************");
            var startTasks = _processors.Values.Select(processor => processor.StartProcessingAsync(cancellationToken));
            await Task.WhenAll(startTasks);
        }

        public async Task StopProcessingAsync()
        {
            //currently processors is empty because the singleton is not working.
            var stopTasks = _processors.Values.Select(processor => processor.StopProcessingAsync());
            await Task.WhenAll(stopTasks);
        }
    }
}
