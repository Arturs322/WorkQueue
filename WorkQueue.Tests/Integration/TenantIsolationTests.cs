using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using WorkQueue.Tests.Helpers;
using Xunit;

namespace WorkQueue.Tests.Integration;

public class TenantIsolationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TenantIsolationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task User_CanOnlySeeOwnOrganizationItems()
    {
        await TestAuthHelper.Authenticate(_client, "memberA@test.com");

        var response = await _client.GetAsync("/api/work-items");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task User_CannotAccessOtherTenantItem()
    {
        await TestAuthHelper.Authenticate(_client, "memberA@test.com");

        var response = await _client.GetAsync($"/api/work-items/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}