namespace WorkQueue.Application.DTO.Auth
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
