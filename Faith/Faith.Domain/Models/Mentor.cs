namespace Faith.Core.Models
{
    public class Mentor : MemberProfile
    {
        public Mentor() { }

        public Mentor(MemberProfile profile)
        {
            MemberId = profile.MemberId;
            FirstName = profile.FirstName;
            LastName = profile.LastName;
            Gender = profile.Gender;
            BirthDate = profile.BirthDate;
            Gender = profile.Gender;
        }

        public int Id { get; set; }

        public ICollection<Student> Students { get; set; } = null!;
        public ICollection<Message> ArchivedMessages { get; set; } = null!;
    }
}