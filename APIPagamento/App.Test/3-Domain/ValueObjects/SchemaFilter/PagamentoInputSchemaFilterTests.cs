using Domain.Entities;
using Domain.Entities.Output;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Domain.ValueObjects.Tests
{
    public class PagamentoInputSchemaFilterTests
    {
        private readonly PagamentoInputSchemaFilter _PagamentoSchemaFilter;

        public PagamentoInputSchemaFilterTests()
        {
            _PagamentoSchemaFilter = new PagamentoInputSchemaFilter();
        }

        [Trait("Categoria", "SchemaFilter")]
        [Fact(DisplayName = "Aplicar Define Exemplo para Schema de Pagamento")]
        public void Aplicar_DefineExemploParaSchemaPagamento()
        {
            // Arrange
            var schema = new OpenApiSchema();
            var context = new SchemaFilterContext(typeof(Pagamento), null, null);

            // Act
            _PagamentoSchemaFilter.Apply(schema, context);

            // Assert
            Assert.NotNull(schema.Example);
        }

        [Trait("Categoria", "SchemaFilter")]
        [Fact(DisplayName = "Aplicar Define Exemplo para Schema de PagamentoOutput")]
        public void Aplicar_DefineExemploParaSchemaPagamentoOutput()
        {
            // Arrange
            var schema = new OpenApiSchema();
            var context = new SchemaFilterContext(typeof(PagamentoOutput), null, null);

            // Act
            _PagamentoSchemaFilter.Apply(schema, context);

            // Assert
            Assert.Null(schema.Example);
        }

    }
}
