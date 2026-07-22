using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using WorkQueue.Application.DTO.WorkItem;
using WorkQueue.Tests.Helpers;
using Xunit;

namespace WorkQueue.Tests.Integration;

public class WorkItemTransitionTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public WorkItemTransitionTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task InvalidTransition_ReturnsBadRequest()
    {
        await TestAuthHelper.Authenticate(_client, "memberA@test.com");

        var createResponse = await _client.PostAsJsonAsync(
                "/api/work-items",
                new
                {
                    title = "Test work item",
                    description = "Testing transition",
                    priority = 1
                });

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var created = await createResponse.Content.ReadFromJsonAsync<WorkItemDto>();

        var response = await _client.PostAsJsonAsync(
                $"/api/work-items/{created!.Id}/transition",
                new
                {
                    status = 3
                });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}