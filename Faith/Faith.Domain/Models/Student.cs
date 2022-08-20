namespace Faith.Core.Models
{
    public class Student : MemberProfile
    {
        public int Id { get; set; }

        public ICollection<Mentor> Mentors { get; set; } = null!;
        public ICollection<Message> Messages { get; set; } = null!;

        public Student() { }

        public Student(MemberProfile profile)
        {
            MemberId = profile.MemberId;
            FirstName = profile.FirstName;
            LastName = profile.LastName;
            Gender = profile.Gender;
            BirthDate = profile.BirthDate;
            Gender = profile.Gender;
        }
    }
}