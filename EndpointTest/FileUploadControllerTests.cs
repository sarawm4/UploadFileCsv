using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
namespace EndpointTest
{
    public class FileUploadControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public FileUploadControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task UploadFileQueue1_ReturnsOk()
        {
            // Arrange
            var content = new MultipartFormDataContent();
            var bytes = "Code,Name,Value\n001,Widget A,10\n002,Widget B,5"u8.ToArray();
            content.Add(new ByteArrayContent(bytes), "file", "test.csv");

            // Act
            var response = await _client.PostAsync("/api/FileUpload/UploadQueue1", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("File uploaded and SaveDB In Queue1.", responseString);
        }
        [Fact]
        public async Task UploadFileQueue2_ReturnsOk()
        {
            // Arrange
            var content = new MultipartFormDataContent();
            var bytes = "Code,Name,Value\n001,Widget A,10\n002,Widget B,5"u8.ToArray();
            content.Add(new ByteArrayContent(bytes), "file", "test.csv");

            // Act
            var response = await _client.PostAsync("/api/FileUpload/UploadQueue2", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("File uploaded and processed.", responseString);
        }
        [Fact]
        public async Task GetSwagger_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/swagger/v1/swagger.json");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("My API", responseString);
            Assert.Contains("/api/UploadFileApi/all", responseString);
            Assert.Contains("/api/UploadFileApi/upload", responseString);
            Assert.Contains("/api/UploadFileApi/{code}", responseString);
            Assert.Contains("/api/UploadFileApi/delete", responseString);
            Assert.Contains("/api/FileUpload/UploadQueue1", responseString);

        }
    }
}