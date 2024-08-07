using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.IService.ICommonService;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.CommonService;

public class Authentication : IAuthentication
{
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int Iterations = 10000;
    private const char Delimiter = ';';
    private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
    private readonly IConfiguration _configuration;

    public Authentication(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool Verify(string HashPassword, string InputPassword)
    {
        var elments = HashPassword.Split(Delimiter);
        var salt = Convert.FromBase64String(elments[0]);
        var hash = Convert.FromBase64String(elments[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(InputPassword, salt, Iterations, _hashAlgorithmName, KeySize);

        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public string GenerateToken(Account account)
    {
        var secretKey = _configuration["Jwt:Key"];
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var secretKryByte = Encoding.UTF8.GetBytes(secretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, account.Username!),
            new("Id", account.Id.ToString()),
            new(ClaimTypes.Role, account.Role!)
        };

        if (!string.IsNullOrEmpty(account.Avatar))
            claims.Add(new Claim("Avatar", account.Avatar));
        else
            claims.Add(new Claim("Avatar", ""));

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(secretKryByte), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescription);
        return jwtTokenHandler.WriteToken(token);
    }
}