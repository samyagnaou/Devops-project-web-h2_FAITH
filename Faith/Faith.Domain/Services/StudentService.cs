using Faith.Core.Interfaces;
using Faith.Core.Models;

namespace Faith.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMentorService _mentorService;

        public StudentService(
            IStudentRepository studentRepository,
            IMentorService mentorService)
        {
            _studentRepository = studentRepository;
            _mentorService = mentorService;
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
            => await _studentRepository.ListAllAsync();

        public async Task<bool> CreateStudentAndAddToGroup(
            string mentorUserId,
            MemberProfile profile)
        {
            var (isCreated, student) = await CreateNewStudent(profile);
            if (!isCreated)
                return false;
            await _mentorService.AddStudentToGroup(mentorUserId, student!);
            return true;
        }

        public async Task<(bool, Student?)> CreateNewStudent(MemberProfile profile)
        {
            var student = new Student(profile);
            try
            {
                student = await _studentRepository.AddAsync(student);
                return (true, student);
            }
            catch (Exception) { }
            return (false, null);
        }

        public async Task<Student?> GetStudentByUserId(string userId)
            => await _studentRepository.GetByUserId(userId);

        public async Task<bool> UpdateMemberProfile(string userId, MemberProfile profile)
        {
            var student = await _studentRepository.GetByUserId(userId);
            if (student == null)
                return false;
            student.FirstName = profile.FirstName;
            student.LastName = profile.LastName;
            student.BirthDate = profile.BirthDate;
            try
            {
                await _studentRepository.UpdateAsync(student);
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}