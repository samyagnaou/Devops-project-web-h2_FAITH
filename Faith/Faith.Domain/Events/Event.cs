using Ardalis.GuardClauses;
using Faith.Domain.Common;
using Faith.Domain.Posts;
using Faith.Domain.Users;

namespace Faith.Domain.Events;

public class Event : Entity
{
    public EventType EventType { get; set; }
    public User From { get; set; }
    public DateTime Date { get; set; }
    public Post Post { get; set; }

    public Event()
    {
    }

    public Event(EventType eventType, User from, Post post)
    {
        EventType = Guard.Against.Null(eventType, nameof(eventType));
        From = Guard.Against.Null(from, nameof(from));
        Date = DateTime.UtcNow;
        Post = Guard.Against.Null(post, nameof(post));
    }
}