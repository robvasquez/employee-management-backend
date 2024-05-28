using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeManagement.Infrastructure.Data.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Application.Services;

public class AuthenticationService
{
    private readonly int _jwtLifespan;
    private readonly string _jwtSecret;
    private readonly UserRepository _userRepository;

    public AuthenticationService(UserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtSecret = configuration["Jwt:Secret"];
        _jwtLifespan = int.Parse(configuration["Jwt:Lifespan"]);
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password)) return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtLifespan),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<bool> RegisterAsync(string username, string password, string role)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(username);
        if (existingUser != null) return false;

        var user = new User
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };

        await _userRepository.AddUserAsync(user);
        return true;
    }
}