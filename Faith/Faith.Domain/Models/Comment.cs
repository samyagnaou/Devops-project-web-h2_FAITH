namespace Faith.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int MessageId { get; set; }
        public Message Message { get; set; } = null!;

        public int? MentorId { get; set; }
        public Mentor? Mentor { get; set; }

        public int? StudentId { get; set; }
        public Student? Student { get; set; }

	}
}