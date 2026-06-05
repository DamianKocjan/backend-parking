using System.Security.Claims;
using AppCore.Dtos;
using AppCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("/api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>Logowanie — zwraca access token i refresh token.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var response = await authService.LoginAsync(dto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>Odświeżenie access tokenu.</summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
    {
        try
        {
            var response = await authService.RefreshTokenAsync(dto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>Wylogowanie — unieważnia refresh token.</summary>
    [HttpPost("revoke")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Revoke([FromBody] string refreshToken)
    {
        try
        {
            await authService.RevokeTokenAsync(refreshToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Dane zalogowanego użytkownika.</summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public IActionResult Me()
    {
        // Claims z tokenu — informacje o użytkowniku pobrane z tokenu
        // porównaj z kodem metody GenerateAccessToken w AuthService
        var user = new UserDto
        {
            Id         = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
            Email      = User.FindFirstValue(ClaimTypes.Email)!,
            FirstName  = User.FindFirstValue(ClaimTypes.GivenName)!,
            LastName   = User.FindFirstValue(ClaimTypes.Surname)!,
            Department = User.FindFirstValue("department")!,
            Roles      = User.FindAll(ClaimTypes.Role).Select(c => c.Value)
        };

        return Ok(user);
    }
}