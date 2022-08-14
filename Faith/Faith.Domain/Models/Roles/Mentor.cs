namespace Faith.Core.Models.Roles;

public class Mentor : Member
{
    public int Id { get; set; }

    public ICollection<Student> Students { get; set; } = null!;
}