using Ardalis.GuardClauses;
using Faith.Domain.Common;

namespace Faith.Domain.Users;

public class User : Entity
{
    public Username Name { get; private set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }

    private User()
    {

    }
    public User(Username name, string email, DateTime dateOfBirth, Gender gender)
    {
        Name = Guard.Against.Null(name, nameof(name));
        Email = Guard.Against.Null(email, nameof(email));
        DateOfBirth = Guard.Against.Null(dateOfBirth, nameof(dateOfBirth));
        Gender = Guard.Against.Null(gender, nameof(gender));
        //Role = nameof(User);
    }

    public string GetUsername()
    {
        return this.Name.Firstname + ' ' + this.Name.Lastname;
    }
}