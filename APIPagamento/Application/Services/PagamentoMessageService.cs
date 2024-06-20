using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Services
{
    public class PagamentoMessageService : IPagamentoMessageService
    {
        private readonly IPagamentoMessageQueue _pagamentoMessageQueue;
        private readonly IPagamentoService _pagamentoService;
        private readonly ILogger<PagamentoMessageService> _logger;

        public PagamentoMessageService(IPagamentoMessageQueue pagamentoMessageQueue, IPagamentoService pagamentoService,  ILogger<PagamentoMessageService> logger) 
        { 
            _pagamentoMessageQueue = pagamentoMessageQueue;
            _pagamentoService = pagamentoService;
            _logger = logger;
        }

        public async Task PublicarMensagem(string message)
        {
            throw new NotImplementedException();
        }

        public async Task ReceberMensagem()
        {            
            string mensagem = await _pagamentoMessageQueue.ReceberMensagem();
            PagamentoInput pagamentoInput = JsonSerializer.Deserialize<PagamentoInput>(mensagem);
            pagamentoInput.DataPagamento = System.DateTime.Now;
            await _pagamentoService.ProcessarPagamento(pagamentoInput);            
        }

        public virtual void Dispose()
        {            
            GC.SuppressFinalize(this);
        }
    }
}
