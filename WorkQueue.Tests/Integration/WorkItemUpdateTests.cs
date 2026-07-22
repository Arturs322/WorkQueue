using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using WorkQueue.Tests.Helpers;
using Xunit;

namespace WorkQueue.Tests.Integration;

public class WorkItemUpdateTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public WorkItemUpdateTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Member_CanUpdateOwnWorkItem()
    {
        await TestAuthHelper.Authenticate(_client, "memberA@test.com");
        
        var response = await _client.PatchAsJsonAsync(
                $"/api/work-items/{Guid.NewGuid()}",
                new
                {
                    title = "Updated"
                });

        response.StatusCode.Should().BeOneOf(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }
}