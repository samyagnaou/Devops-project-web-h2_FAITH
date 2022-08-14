using Faith.Core.Models.Roles;

namespace Faith.Core.Models;

public class Comment
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int PostId { get; set; }
    public Message Post { get; set; } = null!;

    public int MentorId { get; set; }
    public Mentor Mentor { get; set; } = null!;
}