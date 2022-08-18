using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Faith.Core.Interfaces;
using Faith.Core.Models;
using Faith.Core.Models.Roles;
using Faith.Server.Filters;
using Faith.Shared;
using Faith.Shared.Models;
using Faith.Shared.Models.Requests;
using Faith.Shared.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Faith.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Administrator")]
    public class AccountsController : ApiControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStudentService _studentService;
        private readonly IMentorService _mentorService;
        private readonly IConfigurationSection _jwtSettings;

        public AccountsController(IStudentService studentService, IMentorService mentorService, UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _studentService = studentService;
            _mentorService = mentorService;
            _jwtSettings = config.GetSection("JWTSettings");
        }

        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtoList = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? string.Empty
                };
                userDtoList.Add(userDto);
            }
            return userDtoList;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized(new UserLoginResponse { ErrorMessage = "Invalid Authentication" });

            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaimsAsync(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new UserLoginResponse { IsAuthSuccessful = true, Token = token });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterUserRequest request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest();

            var (user, errors) = await CreateUserWithRole(request.Email, request.Password, Roles.Student);
            if (errors.Any())
            {
                return UnprocessableEntity(new RegisterUserResponse
                {
                    IsSuccessfulRegistration = false,
                    Errors = errors
                });
            }

            var isMemberCreated = await CreateMember(user.Id, Roles.Student, new());
            if (!isMemberCreated)
            {
                await _userManager.DeleteAsync(user);
                return UnprocessableEntity(new RegisterUserResponse
                {
                    IsSuccessfulRegistration = false,
                    Errors = errors
                });
            }

            return StatusCode(201);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Mentor}")]
        public async Task<IActionResult> CreateUserWithRole([FromBody] CreateUserWithRoleRequest request)
        {
            if (User.IsInRole(Roles.Mentor) &&
                !request.Role.Equals(Roles.Student, StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new RegisterUserResponse
                {
                    IsSuccessfulRegistration = false,
                    Errors = new List<string> { $"Mentor cannot create user with {request.Role}" }
                });
            }

            var (user, errors) = await CreateUserWithRole(request.Email, request.Password, request.Role);
            if (errors.Any())
            {
                return UnprocessableEntity(new RegisterUserResponse
                {
                    IsSuccessfulRegistration = false,
                    Errors = errors
                });
            }

            var isMemberCreated = await CreateMember(user.Email, request.Role, request.Profile);
            if (!isMemberCreated)
            {
                await _userManager.DeleteAsync(user);
                return UnprocessableEntity(new RegisterUserResponse
                {
                    IsSuccessfulRegistration = false,
                    Errors = errors
                });
            }

            return StatusCode(201);
        }

        private async Task<(IdentityUser, IEnumerable<string>)> CreateUserWithRole(
            string email,
            string password,
            string role)
        {
            var user = new IdentityUser { UserName = email, Email = email };

            var result = await _userManager.CreateAsync(user, password);
            // herhaling van code terugdringen
            if (!result.Succeeded)

                return (user, result.Errors.Select(e => e.Description));

            result = await _userManager.AddToRoleAsync(user, role);

            return (user, result.Errors.Select(e => e.Description));
        }

        private async Task<bool> CreateMember(
            string userId,
            string role,
            MemberProfile profile)
        {
            bool isMemberCreated = true;
            Student? student = null;
            profile.MemberId = userId;

            switch (role)
            {
                case Roles.Student:
                    if (User.IsInRole(Roles.Mentor))
                        isMemberCreated = await _studentService
                            .CreateStudentAndAddToGroup(User!.Identity!.Name!, profile);
                    else
                        (isMemberCreated, student) = await _studentService.CreateNewStudent(profile);
                    break;
                case Roles.Mentor:
                    isMemberCreated = await _mentorService.CreateNewMentor(profile);
                    break;
            }
            return isMemberCreated;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }


        private async Task<List<Claim>> GetClaimsAsync(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.GetSection("validIssuer").Value,
                audience: _jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.GetSection("expiryInMinutes").Value)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }
    }
}
