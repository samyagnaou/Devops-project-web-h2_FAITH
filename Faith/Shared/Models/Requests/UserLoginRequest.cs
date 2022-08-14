﻿using System.ComponentModel.DataAnnotations;

namespace Faith.Shared.Models.Requests;

public class UserLoginRequest
{
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = null!;
}