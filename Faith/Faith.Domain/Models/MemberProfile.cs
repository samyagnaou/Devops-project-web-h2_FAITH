namespace Faith.Core.Models
{
    public class MemberProfile
    {
        public string MemberId { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}