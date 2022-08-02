using Faith.Domain.Common;

namespace Faith.Domain.Posts;

public class Post : Entity
{
    public string Message { get; set; }
    public string ImageUrl { get; set; }

    public Post()
    {
    }

    public Post(string message, int id)
    {
        Message = message;
        Id = id;
    }

    public Post(string message, string imageUrl, int id)
    {
        Message = message;
        ImageUrl = imageUrl;
        Id = id;
    }
}