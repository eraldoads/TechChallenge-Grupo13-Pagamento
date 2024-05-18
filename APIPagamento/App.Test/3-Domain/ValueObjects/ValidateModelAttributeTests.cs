using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using System.Net;
using Xunit;

namespace Domain.ValueObjects.Tests
{
    public class ValidateModelAttributeTests
    {
        private readonly AjustaDataHoraLocal _filter;

        public ValidateModelAttributeTests()
        {
            _filter = new AjustaDataHoraLocal();
        }

        [Trait("Categoria", "AjustaDataHoraLocal")]
        [Fact(DisplayName = "Deve definir o resultado do contexto como um objeto de problema de validação com código 412")]
        public void OnActionExecuting_InvalidModelState_ShouldSetResultAsValidationProblemWithStatusCode412()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var context = new ActionExecutingContext(
                new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);
            context.ModelState.AddModelError("Test", "Test error");

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            var result = Assert.IsType<ObjectResult>(context.Result);
            Assert.Equal((int)HttpStatusCode.PreconditionFailed, result.StatusCode);
        }
    }
}
