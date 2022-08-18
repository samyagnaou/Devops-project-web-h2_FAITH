namespace Faith.Core.Models.Roles;

public class Mentor : MemberProfile
{
    public int Id { get; set; }

    public ICollection<Student> Students { get; set; } = null!;

    public Mentor() { }

    public Mentor(MemberProfile profile)
    {
        FirstName = profile.FirstName;
        LastName = profile.LastName;
        Gender = profile.Gender;
        BirthDate = profile.BirthDate;
        Gender = profile.Gender;
    }
}