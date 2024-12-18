using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using E_CommerceSystem.Models.DTOs;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace E_CommerceSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }



        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="inputUser">User input DTO</param>
        /// <returns>Result of registration</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] InputUserDTO inputUser)
        {
            try
            {
                var user = new User
                {
                    Name = inputUser.Name,
                    Email = inputUser.Email,
                    Phone = inputUser.Phone,
                    Role = "User", // Default role
                    Password = inputUser.Password
                };

                _userService.AddUser(user);
                return Ok(new { Message = "User registered successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Login a user and return a JWT token.
        /// </summary>
        /// <param name="loginDTO">User login credentials</param>
        /// <returns>JWT Token</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginUserDTO loginDTO)
        {
            var user = _userService.GetUserByEmailAndPassword(loginDTO.Email, loginDTO.Password);
            if (user != null)
            {
                var token = GenerateJwtToken(user.Id.ToString(), user.Name, user.Role, user.Email);
                return Ok(new { Token = token, Role = user.Role, Message = "Login successful." });
            }
            return Unauthorized(new { Error = "Invalid email or password." });
        }



        /// <summary>
        /// Get user details by ID (Authenticated Users Only).
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details</returns>
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUserDetails(int id)
        {
            var loggedInUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (loggedInUserId != id.ToString() && User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != "Admin")
            {
                return Forbid();
            }

            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound(new { Error = "User not found." });

            var outputUser = new OutputUserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role
            };
            return Ok(outputUser);
        }

        [NonAction]
        public string GenerateJwtToken(string userId, string username, string role, string email)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("JWT secret key is not configured.");
            }

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.UniqueName, username),
        new Claim(ClaimTypes.Role, role),
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
