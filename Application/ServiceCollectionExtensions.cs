using Application.Builders;
using Core.Interfaces;
using Infrastructure.Services;

namespace Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsumerServiceBusConnection(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<ServiceBusConnectionBuilder> configure)
        {

            //var serviceBusManager = services.BuildServiceProvider().GetRequiredService<IServiceBusManager>();
            var serviceProvider = services.BuildServiceProvider();
            var builder = new ServiceBusConnectionBuilder(ServiceBusManager.Instance, serviceProvider, configuration);
            configure(builder);
            return services;
        }
    }
}
