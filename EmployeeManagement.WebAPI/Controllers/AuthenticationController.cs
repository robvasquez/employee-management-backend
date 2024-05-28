using EmployeeManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly AuthenticationService _authenticationService;

    public AuthenticationController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
    {
        var token = await _authenticationService.AuthenticateAsync(request.Username, request.Password);
        if (token == null) return Unauthorized();

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthenticationRequest request)
    {
        var result = await _authenticationService.RegisterAsync(request.Username, request.Password, "user");
        if (!result) return BadRequest(new { message = "Username already exists" });

        return Ok();
    }
}