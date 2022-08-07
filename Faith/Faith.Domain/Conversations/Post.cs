using Faith.Domain.Common;

namespace Faith.Domain.Conversations;

public class Post : Entity
{
    public string Message { get; set; }
    public string? Image { get; set; }
    public string? Hyperlink { get; set; }
    public Reply? Reply { get; set; }

    public Post()
    {
    }

    public Post(string message)
    {
        Message = message;
    }


    public Post(string message, string image)
    {
        Message = message;
        Image = image;
    }

    public Post(string message, string image, string hyperlink)
    {
        Message = message;
        Image = image;
        Hyperlink = hyperlink;
    }

    public Post(string message, Reply reply)
    {
        Message = message;
        Reply = reply;
    }

    public Post(string message,string image, string hyperlink, Reply reply)
    {
        Message = message;
        Image = image;
        Hyperlink = hyperlink;
        Reply = reply;
    }

    public Post(string message, string hyperlink, Reply reply)
    {
        Message = message;
        Hyperlink = hyperlink;
        Reply = reply;
    }
}