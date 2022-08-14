namespace Faith.Core.Models.Roles;

public class Student : Member
{
    public int Id { get; set; }

    public ICollection<Mentor> Mentors { get; set; } = null!;
    public ICollection<Message> Messages { get; set; } = null!;
}