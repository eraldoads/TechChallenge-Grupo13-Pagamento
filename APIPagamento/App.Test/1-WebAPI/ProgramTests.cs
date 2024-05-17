using App.Test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using Xunit;

namespace APIPagamento.Tests
{
    public class ProgramTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ProgramTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetProdutos_ReturnsOk()
        {
            // Arrange
            var expectedStatusCode = HttpStatusCode.NotFound;

            // Act
            var response = await _client.GetAsync("/produtos");

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public void AddControllers_UsesNewtonsoftJson()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddControllers().AddNewtonsoftJson();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var mvcOptions = serviceProvider.GetService<IOptions<MvcNewtonsoftJsonOptions>>();
            Assert.NotNull(mvcOptions);
            Assert.Equal(Formatting.None, mvcOptions.Value.SerializerSettings.Formatting);
            Assert.Equal(DateFormatHandling.IsoDateFormat, mvcOptions.Value.SerializerSettings.DateFormatHandling);
            Assert.Equal(Formatting.None, mvcOptions.Value.SerializerSettings.Formatting);
            Assert.Equal(DateFormatHandling.IsoDateFormat, mvcOptions.Value.SerializerSettings.DateFormatHandling);
        }
    }
}
