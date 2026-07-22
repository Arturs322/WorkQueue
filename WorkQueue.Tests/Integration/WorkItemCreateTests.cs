using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using WorkQueue.Tests.Helpers;
using Xunit;

namespace WorkQueue.Tests.Integration;

public class WorkItemCreateTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public WorkItemCreateTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Member_CanCreateWorkItem()
    {
        await TestAuthHelper.Authenticate(_client, "memberA@test.com");

        var response = await _client.PostAsJsonAsync(
                "/api/work-items",
                new
                {
                    title = "New task",
                    description = "test",
                    priority = 1
                });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateWithoutTitle_ReturnsBadRequest()
    {
        await TestAuthHelper.Authenticate(_client, "memberA@test.com");

        var response = await _client.PostAsJsonAsync(
                "/api/work-items",
                new
                {
                    description = "missing title",
                    priority = 1
                });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}