namespace WorkQueue.Application.DTO.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public UserProfile Profile { get; set; }
    }
}
