namespace Faith.Shared.Models.Responses;

public class UserLoginResponse
{
    public bool IsAuthSuccessful { get; set; }
    public string ErrorMessage { get; set; } = null!;
    public string Token { get; set; } = null!;
}