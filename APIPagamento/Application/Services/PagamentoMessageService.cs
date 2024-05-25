using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Threading.Channels;

namespace Application.Services
{
    public class PagamentoMessageService : IPagamentoMessageService
    {
        private readonly IPagamentoMessageQueue _pagamentoMessageQueue;
        private readonly ILogger<PagamentoMessageService> _logger;

        public PagamentoMessageService(IPagamentoMessageQueue pagamentoMessageQueue, ILogger<PagamentoMessageService> logger) 
        { 
            _pagamentoMessageQueue = pagamentoMessageQueue;
            _logger = logger;
        }

        public async Task PublicarMensagem(string message)
        {
            throw new NotImplementedException();
        }

        public async Task ReceberMensagem()
        {            
            await _pagamentoMessageQueue.ReceberMensagem();
        }

        public virtual void Dispose()
        {            
            GC.SuppressFinalize(this);
        }
    }
}
