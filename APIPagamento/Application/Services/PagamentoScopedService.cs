using Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            _logger.LogInformation("Serviço de escopo de pagamento iniciado.");

            await _pagamentoMessageService.ReceberMensagens();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(1000, cancellationToken); // Aguarda 1 segundo antes de tentar receber outra mensagem
                }
                catch (OperationCanceledException)
                {
                    // Se o cancellationToken for cancelado, sair do loop
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao receber mensagem.");
                    // Lidar com a exceção conforme necessário, como retry ou logging
                }
            }

            _logger.LogInformation("Serviço de escopo de pagamento encerrado.");
        }
    }
}
