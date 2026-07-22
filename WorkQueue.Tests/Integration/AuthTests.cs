using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace WorkQueue.Tests.Integration;

public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        var response = await _client.PostAsJsonAsync(
                "/api/auth/login",
                new
                {
                    email = "memberA@test.com",
                    password = "Password123!"
                });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        result!.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
    {
        var response = await _client.PostAsJsonAsync(
                "/api/auth/login",
                new
                {
                    email = "memberA@test.com",
                    password = "wrong"
                });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    public class LoginResponse
    {
        public string Token { get; set; } = null!;
    }
}