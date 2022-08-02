namespace Faith.Domain.Users;

public class Youth : User
{
    public Youth(Username name, Gender gender, string email, DateTime dateOfBirth) 
        : base(name, gender, email, dateOfBirth)
    {
    }
}