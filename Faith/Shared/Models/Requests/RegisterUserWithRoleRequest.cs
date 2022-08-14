using Faith.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Faith.Shared.Models.Requests;

public class RegisterUserWithRoleRequest : RegisterUserRequest
{
    public MemberDetails MemberDetails { get; set; } = null!;

    [RegularExpression($"{Roles.Admin}|{Roles.Mentor}|{Roles.Student}")]
    public string Role { get; set; } = null!;
}