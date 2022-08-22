using Faith.Core.Models;

namespace Faith.Shared.Models.Requests
{
    public class ChangeProfileDetailsRequest
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
    }
}