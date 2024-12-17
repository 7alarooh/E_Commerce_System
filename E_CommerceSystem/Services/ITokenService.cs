using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}