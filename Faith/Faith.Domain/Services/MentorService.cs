using Faith.Core.Interfaces;
using Faith.Core.Models;
using Faith.Core.Models.Roles;

namespace Faith.Core.Services;

public class MentorService : IMentorService
{
    private readonly IRepository<Mentor> _mentorRepository;

    public MentorService(IRepository<Mentor> mentorRepository)
    {
        _mentorRepository = mentorRepository;
    }

    public async Task<bool> CreateNewMentor(MemberDetails memberDetails)
    {
        var mentor = new Mentor(memberDetails);
        try
        {
            await _mentorRepository.AddAsync(mentor);
            return true;
        }
        catch (Exception) { }
        return false;
    }
}