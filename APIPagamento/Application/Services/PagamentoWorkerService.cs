using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class PagamentoWorkerService : BackgroundService
    {
        private readonly ILogger<PagamentoWorkerService> _logger;
        public IServiceProvider Services { get; }

        public PagamentoWorkerService(IServiceProvider services, ILogger<PagamentoWorkerService> logger) 
        {
            Services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            using (var scope = Services.CreateScope())
            {
                IPagamentoScopedService scopedProcessingService = scope.ServiceProvider.GetRequiredService<IPagamentoScopedService>();
                await scopedProcessingService.DoWork(cancellationToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
