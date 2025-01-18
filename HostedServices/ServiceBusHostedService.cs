using Core.Interfaces.Services;
using Core.Settings;
using Microsoft.Extensions.Options;


namespace Microservices.HostedServices
{
    public class ServiceBusHostedService : CronJobServiceBase
    {
        private readonly IServiceProvider _serviceProvider;
        private AsyncServiceScope _scope;
        private IHostedServiceTask _taskService;

        public ServiceBusHostedService(
            IOptions<ServiceBusHostedServiceSettings> hostedServiceSettings
            , ILogger<CronJobServiceBase> log,
            IServiceProvider serviceProvider) : base(hostedServiceSettings, log)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteTaskAsync(CancellationToken cancellationToken)
        {
            _scope = _serviceProvider.CreateAsyncScope();
            _taskService = _scope.ServiceProvider.GetRequiredService<IEventServiceBusServiceTask>();
            await _taskService.StartAsync(cancellationToken);
        }

        protected override async Task DisposeScope()
        {
            await _taskService.StopAsync(CancellationToken.None);
            await _scope.DisposeAsync();
        }
    }
}
