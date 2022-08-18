using System.ComponentModel.DataAnnotations;
using Faith.Core.Models;

namespace Faith.Shared.Models.Requests;

public class CreateUserWithRoleRequest : RegisterUserRequest
{
    public MemberProfile Profile { get; set; } = new();

    [RegularExpression($"{Roles.Admin}|{Roles.Mentor}|{Roles.Student}")]
    public string Role { get; set; } = null!;
}