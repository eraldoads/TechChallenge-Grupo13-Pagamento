using Data.Context;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Data.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly IMongoCollection<Pagamento> _pagamentoCollection;
        private readonly IMongoCollection<PagamentoInput> _pagamentoInputCollection;

        public PagamentoRepository(MongoDBContext context)
        {
            _pagamentoCollection = context.Pagamento;
            _pagamentoInputCollection = context.PagamentoInput;
        }

        public async Task<Pagamento?> GetPagamentoByIdPedido(int idPedido)
        {
            // Caso encontre mais de um registro pega sempre o ultimo.
            return await _pagamentoCollection.Find(p => p.IdPedido == idPedido)
                                             .SortByDescending(p => p.DataPagamento)
                                             .FirstOrDefaultAsync();
        }


        public async Task<PagamentoInput> PostPagamento(PagamentoInput pagamentoInput)
        {
            await _pagamentoInputCollection.InsertOneAsync(pagamentoInput);
            return pagamentoInput;
        }

        public async Task<Pagamento> PutPagamento(Pagamento pagamento)
        {
            var filter = Builders<Pagamento>.Filter.Eq(p => p.Id, pagamento.Id);
            var update = Builders<Pagamento>.Update.Set(p => p.StatusPagamento, pagamento.StatusPagamento);
            await _pagamentoCollection.UpdateOneAsync(filter, update);
            return pagamento;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Dispose any resources if needed
        }

        public async Task<Pagamento?> GetPagamentoById(string idPagamento)
        {
            // Caso encontre mais de um registro pega sempre o ultimo.
            return await _pagamentoCollection.Find(p => p.Id == idPagamento)
                                             .SortByDescending(p => p.DataPagamento)
                                             .FirstOrDefaultAsync();
        }
    }
}
