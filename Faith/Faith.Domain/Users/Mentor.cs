namespace Faith.Domain.Users;

public class Mentor : User
{
    public Mentor(Username name, Gender gender, string email, DateTime dateOfBirth) 
        : base(name, gender, email, dateOfBirth)
    {
    }
}