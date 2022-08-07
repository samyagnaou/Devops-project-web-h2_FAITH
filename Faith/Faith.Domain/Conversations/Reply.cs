using Faith.Domain.Common;

namespace Faith.Domain.Conversations;

public class Reply : Entity
{
    public string Message { get; set; }
    public List<Reply>? Replies { get; set; }

    public Reply()
    {
    }

    public Reply(string message)
    {
        Message = message;
    }

    public Reply(string message, List<Reply> replies)
    {
        Message = message;
        Replies = replies;
    }

    public void AddReply(Reply reply)
    {
        this.Replies.Add(reply);
    }

    public string CountOfReplies()
    {
        return this.Replies.Count.ToString();
    }


}