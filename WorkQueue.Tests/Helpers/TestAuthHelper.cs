using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WorkQueue.Tests.Helpers;

public static class TestAuthHelper
{
    public static async Task Authenticate(HttpClient client, string email)
    {
        var response = await client.PostAsJsonAsync(
            "/api/auth/login",
            new
            {
                email,
                password = "Password123!"
            });

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<LoginResponse>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                result!.Token);
    }
}

public class LoginResponse
{
    public string Token { get; set; } = null!;
}