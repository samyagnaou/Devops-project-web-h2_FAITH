namespace Faith.Core.Models.Roles;

public class Member
{
    public string MemberId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string ProfilePictureUrl { get; set; } = null!;
}