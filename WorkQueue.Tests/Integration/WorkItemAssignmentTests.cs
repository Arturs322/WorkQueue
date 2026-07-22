using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using WorkQueue.Tests.Helpers;
using Xunit;

namespace WorkQueue.Tests.Integration;

public class WorkItemAssignmentTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public WorkItemAssignmentTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Member_CannotAssign()
    {
        await TestAuthHelper.Authenticate(_client, "memberA@test.com");

        var response = await _client.PostAsJsonAsync(
                $"/api/work-items/{Guid.NewGuid()}/assign",
                new
                {
                    userId = Guid.NewGuid()
                });

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}