using Faith.Core.Interfaces;
using Faith.Core.Models;
using Faith.Core.Models.Roles;

namespace Faith.Core.Services;

public class StudentService : IStudentService
{
    private readonly IRepository<Student> _studentRepository;

    public StudentService(IRepository<Student> studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<bool> CreateNewStudent(MemberDetails memberDetails)
    {
        var student = new Student(memberDetails);
        try
        {
            await _studentRepository.AddAsync(student);
            return true;
        }
        catch (Exception) { }
        return false;
    }
}