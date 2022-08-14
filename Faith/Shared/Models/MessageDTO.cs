namespace Faith.Shared.Models;

public class MessageDTO
{
    public string Text { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}