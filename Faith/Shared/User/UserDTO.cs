using FluentValidation;

namespace Faith.Shared.User;

public static class UserDTO
{
    public class Index
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }

    public class Detail : Index
    {
        public string Email { get; set; }
        public string Gender { get; set; }
    }

    public class Mutate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }


        public class Validator : AbstractValidator<Mutate>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName).NotEmpty().Length(1, 100);
                RuleFor(x => x.LastName).NotEmpty().Length(1, 100);
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            }
        }
    }
}