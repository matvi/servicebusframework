using Application.Builders;
using Core.Interfaces;

namespace Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsumerServiceBusConnection(
            this IServiceCollection services,
            Action<ServiceBusConnectionBuilder> configure)
        {

            var serviceBusManager = services.BuildServiceProvider().GetRequiredService<IServiceBusManager>();
            var builder = new ServiceBusConnectionBuilder(serviceBusManager);
            configure(builder);
            return services;
        }
    }
}
