using Faith.Shared.User;

namespace Faith.Services.Users;

public class UserService : IUserService
{
    public Task<UserResponse.Create> CreateAsync(UserRequest.Create request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(UserRequest.Delete request)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse.Edit> EditAsync(UserRequest.Edit request)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse.GetDetail> GetDetailAsync(UserRequest.GetDetail request)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse.GetIndex> GetIndexAsync(UserRequest.GetIndex request)
    {
        throw new NotImplementedException();
    }
}