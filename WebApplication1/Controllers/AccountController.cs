using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Dtos;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    RoleManager<IdentityRole> roleManager,
    IConfiguration configuration) : ControllerBase // Added IConfiguration
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var roleExists = await roleManager.RoleExistsAsync(model.Role);
        if (!roleExists)
        {
            return BadRequest("Selected role does not exist.");
        }

        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, model.Role);
            return Ok(new { Message = $"User registered successfully with role: {model.Role}" });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null) return Unauthorized("Invalid credentials.");

        var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        if (result.Succeeded)
        {
            var roles = await userManager.GetRolesAsync(user);

            // Generate the actual JWT token
            var token = GenerateJwtToken(user, roles);

            return Ok(new
            {
                Message = "Login Successful",
                Token = token,
                Email = user.Email,
                Roles = roles
            });
        }

        return Unauthorized("Invalid credentials.");
    }

    // Helper method to generate the JWT
    private string GenerateJwtToken(IdentityUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        // Add roles to the token claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}