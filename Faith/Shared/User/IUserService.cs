namespace Faith.Shared.User;

public interface IUserService
{
    Task<UserResponse.GetIndex> GetIndexAsync(UserRequest.GetIndex request);
    Task<UserResponse.GetDetail> GetDetailAsync(UserRequest.GetDetail request);
    //Task<UserResponse.GetRoleByUserId> GetRoleAsync(UserRequest.GetRoleByUserId request);
    Task<UserResponse.Create> CreateAsync(UserRequest.Create request);
    Task<UserResponse.Edit> EditAsync(UserRequest.Edit request);
    Task DeleteAsync(UserRequest.Delete request);
}