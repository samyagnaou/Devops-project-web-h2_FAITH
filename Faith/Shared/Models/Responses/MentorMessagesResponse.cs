using Faith.Core.Models;
namespace Faith.Shared.Models.Responses;

public class MentorMessagesResponse
{
    public IEnumerable<Message>? Messages { get; set; }
    public IEnumerable<Message>? ArchivedMessages { get; set; }
}