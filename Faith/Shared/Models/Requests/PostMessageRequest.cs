namespace Faith.Shared.Models.Requests;

public class PostMessageRequest
{
    public string Text { get; set; } = null!;

    public string? ImageUrl { get; set; }
}