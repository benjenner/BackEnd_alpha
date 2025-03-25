using System.Security.Claims;

namespace Business.Interfaces
{
    public interface ITokenManager
    {
        string GenerateJwtToken(List<Claim> claims);
    }
}