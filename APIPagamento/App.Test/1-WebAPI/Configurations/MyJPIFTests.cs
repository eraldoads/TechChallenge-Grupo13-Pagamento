using JsonPatchSample;
using Microsoft.AspNetCore.Mvc.Formatters;
using Xunit;

namespace App.Test._1_WebAPI.Configurations
{
    public class MyJPIFTests
    {
        [Fact]
        public void GetJsonPatchInputFormatter_ReturnsCorrectType()
        {
            // Act
            var result = MyJPIF.GetJsonPatchInputFormatter();

            // Assert
            Assert.IsType<NewtonsoftJsonPatchInputFormatter>(result);
        }

        [Fact]
        public void GetJsonPatchInputFormatter_ReturnsNotNull()
        {
            // Act
            var result = MyJPIF.GetJsonPatchInputFormatter();

            // Assert
            Assert.NotNull(result);
        }
    }
}
