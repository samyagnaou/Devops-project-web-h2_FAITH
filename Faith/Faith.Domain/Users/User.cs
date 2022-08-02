using Ardalis.GuardClauses;
using Faith.Domain.Common;

namespace Faith.Domain.Users
{
    public class User : Entity
    {
        public Username Name { get; private set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public User(Username name, Gender gender, string email, DateTime dateOfBirth)
        {
            Name = Guard.Against.Null(name, nameof(name));
            Gender = Guard.Against.Null(gender, nameof(gender));
            Email = Guard.Against.Null(email, nameof(email));
            DateOfBirth = Guard.Against.Null(dateOfBirth, nameof(dateOfBirth));
            //Note: Add check for role of the user
        }

        public string GetUsername()
        {
            return this.Name.Firstname + ' ' + this.Name.Lastname;
        }

        //Note: PlacePost function

    }
}