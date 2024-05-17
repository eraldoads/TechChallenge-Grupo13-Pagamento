using App.Test._4_Infrastructure.Context;
using Data.Repository;
using Domain.Base;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Data.Tests.Repository
{
    public class PagamentoRepositoryTests
    {
        private readonly MySQLContextTests _context;
        private readonly PagamentoRepository _repository;

        public PagamentoRepositoryTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySQLContextTests>();
            optionsBuilder.UseInMemoryDatabase(databaseName: "TestDatabase");
            var options = optionsBuilder.Options;

            _context = new MySQLContextTests(options);
            _repository = new PagamentoRepository(_context);
        }

        #region [Get]
        [Trait("Categoria", "PagamentoRepository")]
        [Fact(DisplayName = "GetPagamentoByIdPedido Deve Retornar o Pagamento")]
        public async Task GetPagamentoByIdPedido_RetornaPagamento()
        {
            // Arrange
            _context.Pagamento.RemoveRange(_context.Pagamento);
            await _context.SaveChangesAsync();

            var idPedido = 1;
            var pagamento = new Pagamento
            {
                IdPedido = idPedido,
                StatusPagamento = "Pendente",
                ValorPagamento = 100.0f
            };

            _context.Pagamento.Add(pagamento);
            await _context.SaveChangesAsync();

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
            _context.Pagamento.RemoveRange(_context.Pagamento);
            await _context.SaveChangesAsync();

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
            _context.Pagamento.RemoveRange(_context.Pagamento);
            await _context.SaveChangesAsync();

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
                IdPedido = 1,
                StatusPagamento = "Pendente",
                ValorPagamento = 100.0f
            };

            _context.Pagamento.Add(pagamento);
            await _context.SaveChangesAsync();

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
        [Fact(DisplayName = "Dispose_DeveDescartarContexto")]
        public void Dispose_DeveDescartarContexto()
        {
            // Arrange
            _context.Pagamento.RemoveRange(_context.Pagamento);
            _context.SaveChangesAsync();

            var produto = new Pagamento
            {
                IdPedido = 1,
                StatusPagamento = "Pendente",
                ValorPagamento = 100
            };

            _context.Pagamento.Add(produto);
            _context.SaveChanges();

            // Act
            _repository.Dispose();

            // Assert
            Assert.NotNull(_context);
        }

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