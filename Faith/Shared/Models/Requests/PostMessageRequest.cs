using Microsoft.AspNetCore.Http;

namespace Faith.Shared.Models.Requests;

public class PostMessageRequest
{
    public string Text { get; set; } = null!;

    public IFormFile? ImageFile { get; set; }
}