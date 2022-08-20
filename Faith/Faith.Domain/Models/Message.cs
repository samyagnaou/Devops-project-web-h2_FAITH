namespace Faith.Core.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Text { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public ICollection<Mentor> ArchivedBy { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = null!;
    }
}