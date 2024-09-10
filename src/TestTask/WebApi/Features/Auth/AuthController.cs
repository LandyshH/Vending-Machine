using Domain.Products;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Features.Auth.Requests;

namespace WebApi.Features.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var user = await userManager.FindByNameAsync(request.UserName);

        if (user is null)
        {
            return BadRequest();
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            return BadRequest();
        }

        var claims = new List<Claim> { new(ClaimTypes.Name, request.UserName) };

        var jwt = new JwtSecurityToken(
            issuer: configuration["JWT_ISSUER"]!,
            audience: configuration["JWT_AUDIENCE"]!,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT_KEY"]!)), SecurityAlgorithms.HmacSha256));

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return Ok(token);
    }
}
