using EmployeeManagement.Application.Services;
using EmployeeManagement.Infrastructure.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly AuthenticationService _authenticationService;
    private readonly UserRepository _userRepository;

    public AuthenticationController(AuthenticationService authenticationService, UserRepository userRepository)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
    {
        var token = await _authenticationService.AuthenticateAsync(request.Username, request.Password);
        if (token == null) return Unauthorized("Invalid username or password.");

        var user = await _userRepository.GetUserByUsernameAsync(request.Username);
        if (user == null) return Unauthorized("User not found.");

        return Ok(new { token, userId = user.Id, username = user.Username });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthenticationRequest request)
    {
        var result = await _authenticationService.RegisterAsync(request.Username, request.Password, "user");
        if (!result) return BadRequest(new { message = "Username already exists" });

        return Ok();
    }
}