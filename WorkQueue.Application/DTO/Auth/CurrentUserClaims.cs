namespace WorkQueue.Application.DTO.Auth
{
    public class CurrentUserClaims
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
