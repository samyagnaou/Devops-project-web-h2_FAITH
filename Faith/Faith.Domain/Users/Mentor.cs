using Ardalis.GuardClauses;

namespace Faith.Domain.Users;

public class Mentor : User
{

    public Mentor(Username name, string email, DateTime dateOfBirth, Gender gender)
            : base(name, email, dateOfBirth, gender)
        {

        }
    
}