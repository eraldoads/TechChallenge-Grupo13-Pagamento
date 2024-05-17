using App.Test._4_Infrastructure.Context;
using Data.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Mongo2Go;
using Data.Context;
using MongoDB.Driver;

namespace Data.Tests.Repository
{
    public class PagamentoRepositoryTests : IDisposable
    {
        private readonly MongoDBContextTests _context;
        private readonly PagamentoRepository _repository;
        private readonly MongoDbRunner _runner;

        public PagamentoRepositoryTests()
        {
            // Inicia o MongoDbRunner
            _runner = MongoDbRunner.Start();

            // Cria uma nova instância do MongoDBContext
            var settings = MongoClientSettings.FromUrl(new MongoUrl(_runner.ConnectionString));
            var client = new MongoClient(settings);
            _context = new MongoDBContextTests(client.GetDatabase("TestDatabase"));

            _repository = new PagamentoRepository(_context);
        }

        #region [Get]
        [Trait("Categoria", "PagamentoRepository")]
        [Fact(DisplayName = "GetPagamentoByIdPedido Deve Retornar o Pagamento")]
        public async Task GetPagamentoByIdPedido_RetornaPagamento()
        {
            // Arrange
            await _context.Pagamento.DeleteManyAsync(Builders<Pagamento>.Filter.Empty);

            var idPedido = 1;
            var pagamento = new Pagamento
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                IdPedido = idPedido,
                StatusPagamento = "Pendente",
                ValorPagamento = 100.0f
            };


            await _context.Pagamento.InsertOneAsync(pagamento);

            // Act
            var result = await _repository.GetPagamentoByIdPedido(idPedido);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(idPedido, result.IdPedido);
        }

        [Trait("Categoria", "PagamentoRepository")]
        [Fact(DisplayName = "GetPagamentoByIdPedido Deve Retornar Null")]
        public async Task GetPagamentoByIdPedido_RetornaNull()
        {
            // Arrange
            var idPedido = 2;
            Pagamento pagamento = null;

            // Act
            var result = await _repository.GetPagamentoByIdPedido(idPedido);

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region [POST]
        [Trait("Categoria", "PagamentoRepository")]
        [Fact(DisplayName = "PostPagamento Deve Retornar Pagamento")]
        public async Task PostPagamento_RetornaPagamento()
        {
            // Arrange
            var pagamento = new Pagamento
            {
                IdPedido = 1,
                StatusPagamento = "Pendente",
                ValorPagamento = 100.0f
            };

            // Act
            var result = await _repository.PostPagamento(pagamento);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagamento.IdPedido, result.IdPedido);
        }
        #endregion

        #region [PUT]
        [Trait("Categoria", "PagamentoRepository")]
        [Fact(DisplayName = "PutPagamento Deve Retornar Pagamento")]
        public async Task PutPagamento_RetornaPagamento()
        {
            // Arrange
            var pagamento = new Pagamento
            {
                Id = "1",
                IdPedido = 1,
                StatusPagamento = "Pendente",
                ValorPagamento = 100.0f
            };

            _context.Pagamento.InsertOneAsync(pagamento);

            pagamento.StatusPagamento = "Pago";

            // Act
            var result = await _repository.PutPagamento(pagamento);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagamento.StatusPagamento, result.StatusPagamento);
        }
        #endregion

        #region [Dispose]
        [Trait("Categoria", "PagamentoRepository")]
        [Fact(DisplayName = "Dispose")]
        public void Dispose()
        {
            //act
            _repository.Dispose();
            Assert.NotNull(_context);
        }
        #endregion

    }
}