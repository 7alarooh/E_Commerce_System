namespace E_CommerceSystem.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(string userId, string username, string role, string email);
    }
}