using Authentication.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Handlers;

public class JwtTokenHandler
{
    private readonly IConfiguration _configuration;

    public JwtTokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // Metoden kräver en AppUser samt en sträng som kan vara null
    // public string GenerateToken()
    //{
    // *var key* Token genereras

    // *var claims* Ny lista av typen Claim.
    //      NameIdentifier sätts till AppUser.id

    //      Email sätts till AppUser.id ( ** TA BORT DENNA** )

    // Om roll inte är tom, lägg till en ny claim med rollen

    // ** var tokenDescriptor** skapar en ny SecurityTokenSecriptor

    //      Skapar ett nytt subject sav typen ClaimsIdentity som sätts till claims
    //      Token sätts giltlig i en timme
    //      Skapar en ny SigningCredentials med hjälp av en enkrypterings-algoritm

    // ** var tokenHandler ** sätts till JwtSecurityTokenHandler.
    // ** var token ** här
    public string GenerateToken(AppUser appUser, string? role = null)
    {
        try
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!);
            var issuer = _configuration["Jwt:Issuer"]!;
            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, appUser.Id),
            new(ClaimTypes.Email, appUser.Email!)
        };

            if (role != null)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            return null!;
        }
    }
}