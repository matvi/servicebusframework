
namespace Core.Interfaces.Services
{
    public interface IHostedServiceTask
    {
        public Task StartAsync(CancellationToken cancellationToken);
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
