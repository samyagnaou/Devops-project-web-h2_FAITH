using Faith.Core.Models;

namespace Faith.Core.Interfaces;

public interface IMentorService
{
    Task<bool> CreateNewMentor(MemberDetails memberDetails);
}