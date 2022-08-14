using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Faith.Core.Interfaces;
using Faith.Core.Models;
using Faith.Shared;
using Faith.Shared.Models.Requests;
using Faith.Shared.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IConfiguration _config;
        private readonly IConfigurationSection _jwtSettings;

        public AccountsController(IStudentService studentService, IMentorService mentorService, UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _studentService = studentService;
            _mentorService = mentorService;
            _config = config;
            _jwtSettings = _config.GetSection("JWTSettings");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized(new UserLoginResponse { ErrorMessage = "Invalid Authentication" });

            var signingCredentials = GetSigningCredentials();
            var claims = GetClaimsAsync(user);
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
        public async Task<IActionResult> CreateUserWithRole([FromBody] CreateUserWithRole request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest();

            var (user, errors) = await CreateUserWithRole(request.Email, request.Password, request.Role);
            if (errors.Any())
            {
                return UnprocessableEntity(new RegisterUserResponse
                {
                    IsSuccessfulRegistration = false,
                    Errors = errors
                });
            }

            var isMemberCreated = await CreateMember(user.Id, request.Role, request.MemberDetails);
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
            if (!result.Succeeded)
            {
                return (user, result.Errors.Select(e => e.Description));
            }
            result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return (user, result.Errors.Select(e => e.Description));
            }
            return (user, result.Errors.Select(e => e.Description));
        }

        private async Task<bool> CreateMember(
            string userId,
            string role,
            MemberDetails details)
        {
            bool isMemberCreated = true;
            details.MemberId = userId;

            switch (role)
            {
                case Roles.Student:
                    isMemberCreated = await _studentService.CreateNewStudent(details);
                    break;
                case Roles.Mentor:
                    isMemberCreated = await _mentorService.CreateNewMentor(details);
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
