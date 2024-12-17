using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using E_CommerceSystem.Models.DTOs;

namespace E_CommerceSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(UserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
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
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDTO loginDTO)
        {
            var user = _userService.GetUserByEmailAndPassword(loginDTO.Email, loginDTO.Password);
            if (user != null)
            {
                var token = _tokenService.GenerateToken(user);
                return Ok(new { Token = token, Message = "Login successful." });
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
    }
}
