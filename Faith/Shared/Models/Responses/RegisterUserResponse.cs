namespace Faith.Shared.Models.Responses;

public class RegisterUserResponse
{
    public bool IsSuccessfulRegistration { get; set; }
    public IEnumerable<string> Errors { get; set; } = null!;
}