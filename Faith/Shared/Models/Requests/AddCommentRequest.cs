namespace Faith.Shared.Models.Requests
{
    public class AddCommentRequest
    {
        public int MessageId { get; set; }
        public string Text { get; set; } = null!;
    }
}