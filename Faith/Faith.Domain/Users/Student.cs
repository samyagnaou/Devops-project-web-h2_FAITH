namespace Faith.Domain.Users;

public class Student : User
{

    public Student(Username name, string email, DateTime dateOfBirth, Gender gender)
        : base(name, email, dateOfBirth, gender)
    {

    }
}