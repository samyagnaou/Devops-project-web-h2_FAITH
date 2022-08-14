namespace Faith.Core.Models.Roles;

public class Mentor : MemberDetails
{
    public int Id { get; set; }

    public ICollection<Student> Students { get; set; } = null!;

    public Mentor() { }

    public Mentor(MemberDetails memberDetails)
    {
        FirstName = memberDetails.FirstName;
        LastName = memberDetails.LastName;
        Gender = memberDetails.Gender;
        BirthDate = memberDetails.BirthDate;
        Gender = memberDetails.Gender;
    }
}