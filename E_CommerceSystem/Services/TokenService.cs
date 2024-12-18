using System;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace E_CommerceSystem.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly int _expiryInMinutes;

        public TokenService(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            _secretKey = jwtSettings["SecretKey"] ?? throw new ArgumentException("SecretKey is not configured.");
            if (!int.TryParse(jwtSettings["ExpiryInMinutes"], out _expiryInMinutes))
                throw new ArgumentException("ExpiryInMinutes is not configured or invalid.");
        }

        /// <summary>
        /// Generates a JWT token for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <returns>A JWT token string.</returns>
        public string GenerateJwtToken(string userId, string username, string role, string email)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.UniqueName, username),
        new Claim(ClaimTypes.Role, role),
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiryInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
