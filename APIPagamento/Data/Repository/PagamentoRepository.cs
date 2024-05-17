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

        public PagamentoRepository(MongoDBContext context)
        {
            _pagamentoCollection = context.Pagamento;
        }

        public async Task<Pagamento?> GetPagamentoByIdPedido(int idPedido)
        {
            // Caso encontre mais de um registro pega sempre o ultimo.
            return await _pagamentoCollection.Find(p => p.IdPedido == idPedido)
                                             .SortByDescending(p => p.DataPagamento)
                                             .FirstOrDefaultAsync();
        }


        public async Task<Pagamento> PostPagamento(Pagamento pagamento)
        {
            await _pagamentoCollection.InsertOneAsync(pagamento);
            return pagamento;
        }

        public async Task<Pagamento> PutPagamento(Pagamento pagamento)
        {
            var filter = Builders<Pagamento>.Filter.Eq(p => p.IdPagamento, pagamento.IdPagamento);
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
    }
}
