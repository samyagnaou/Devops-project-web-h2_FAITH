namespace Faith.Core.Models.Roles;

public class Student : MemberDetails
{
    public int Id { get; set; }

    public ICollection<Mentor> Mentors { get; set; } = null!;
    public ICollection<Message> Messages { get; set; } = null!;

    public Student() { }

    public Student(MemberDetails details)
    {
        FirstName = details.FirstName;
        LastName = details.LastName;
        Gender = details.Gender;
        BirthDate = details.BirthDate;
        Gender = details.Gender;
    }
}