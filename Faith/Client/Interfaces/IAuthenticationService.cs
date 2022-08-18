using Faith.Shared.Models.Requests;
using Faith.Shared.Models.Responses;

namespace Faith.Client.Interfaces;

public interface IAuthenticationService
{
    Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request);
    Task<UserLoginResponse> Login(UserLoginRequest request);
    Task Logout();
}