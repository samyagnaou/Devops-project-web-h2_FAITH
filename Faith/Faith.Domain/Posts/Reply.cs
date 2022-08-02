using Faith.Domain.Common;

namespace Faith.Domain.Posts;

public class Reply : Entity
{
    public string Message { get; set; }

    public Reply()
    {
    }

    public Reply(string message, int id)
    {
        Message = message;
        Id = id;
    }

}