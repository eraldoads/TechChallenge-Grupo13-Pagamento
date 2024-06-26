using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class PagamentoScopedService : IPagamentoScopedService
    {
        private readonly IPagamentoMessageService _pagamentoMessageService;
        private readonly ILogger<PagamentoScopedService> _logger;

        public PagamentoScopedService(IPagamentoMessageService pagamentoMessageService, ILogger<PagamentoScopedService> logger) 
        {
            _pagamentoMessageService = pagamentoMessageService;
            _logger = logger;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            await _pagamentoMessageService.ReceberMensagem();
        }
    }
}
