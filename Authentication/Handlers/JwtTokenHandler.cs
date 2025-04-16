using Authentication.Entities;
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

    public string GenerateToken(AppUserEntity appUser, string? role = null)
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
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                if (role == "Admin")
                {
                    claims.Add(new Claim("apiKey", _configuration["SecretKeys:Admin"]!));
                }
                else
                {
                    claims.Add(new Claim("apiKey", _configuration["SecretKeys:Default"]!));
                }
            }

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