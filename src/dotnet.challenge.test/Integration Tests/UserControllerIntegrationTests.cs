using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using dotnet.challenge.api;
using dotnet.challenge.api.Utils.Dtos;
using dotnet.challenge.api.Utils.Forms;
using Microsoft.AspNetCore.Mvc.Testing;

namespace dotnet.challenge.test.Integration_Tests
{
    public class UserControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;

        public UserControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        private StringContent SerializeToJson(object obj)
        {
            var json = JsonSerializer.Serialize(obj, _jsonOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        [Fact]
        public async Task CreateUser_ThenGetUser_ReturnsUser()
        {
            var userForm = new UserForm
            {
                Email = $"integration{Guid.NewGuid()}@test.com",
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = "1990-01-01"
            };

            var content = SerializeToJson(userForm);
            var createResponse = await _client.PostAsync("/api/User", content);
            createResponse.EnsureSuccessStatusCode();

            var createdJson = await createResponse.Content.ReadAsStringAsync();
            var createdUser = JsonSerializer.Deserialize<UserDto>(createdJson, _jsonOptions);

            var getResponse = await _client.GetAsync($"/api/User/{createdUser.Id}");
            getResponse.EnsureSuccessStatusCode();

            var getJson = await getResponse.Content.ReadAsStringAsync();
            var fetchedUser = JsonSerializer.Deserialize<UserDto>(getJson, _jsonOptions);

            Assert.Equal(userForm.Email, fetchedUser.Email);
        }

        [Fact]
        public async Task DeleteUser_RemovesUser()
        {
            var userForm = new UserForm
            {
                Email = $"delete{Guid.NewGuid()}@test.com",
                FirstName = "ToDelete",
                LastName = "User",
                DateOfBirth = "1995-05-05"
            };

            var content = SerializeToJson(userForm);
            var createResponse = await _client.PostAsync("/api/User", content);
            createResponse.EnsureSuccessStatusCode();

            var createdJson = await createResponse.Content.ReadAsStringAsync();
            var createdUser = JsonSerializer.Deserialize<UserDto>(createdJson, _jsonOptions);

            var deleteResponse = await _client.DeleteAsync($"/api/User/{createdUser.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/api/User/{createdUser.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
