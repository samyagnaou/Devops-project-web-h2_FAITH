namespace Faith.Domain
{
    public class User
    {
        public Username Name { get; private set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}