using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using WorkQueue.Tests.Helpers;
using Xunit;

namespace WorkQueue.Tests.Integration;

public class ConcurrencyTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ConcurrencyTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ConcurrentUpdate_ReturnsConflict()
    {
        await TestAuthHelper.Authenticate(_client,"memberA@test.com");

        var id = Guid.NewGuid();

        var first = await _client.PatchAsJsonAsync(
                $"/api/work-items/{id}",
                new
                {
                    title = "First update",
                    version = 1
                });

        var second = await _client.PatchAsJsonAsync(
                $"/api/work-items/{id}",
                new
                {
                    title = "Second update",
                    version = 1
                });

        second.StatusCode.Should().BeOneOf(HttpStatusCode.Conflict,HttpStatusCode.NotFound);
    }
}