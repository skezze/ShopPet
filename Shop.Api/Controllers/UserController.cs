using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shop.Api.Data;
using Shop.Api.Models;
using Shop.Api.Models.ViewModels;

namespace Shop.Api.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class UserController:ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserDbContext _userContext;

    public UserController(IConfiguration configuration, UserDbContext userContext)
    {
        _configuration = configuration;
        _userContext = userContext;
    }
 
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserView userView)
    {
        if (userView.UserName != null && userView.Password != null)
        {
            var userInDb = await _userContext.Users.FirstOrDefaultAsync(
                u => u.UserName == userView.UserName && u.Password == userView.Password);
            if (userInDb==null)
            {
                await _userContext.Users.AddAsync(new User()
                {
                    UserName = userView.UserName,
                    Password = userView.Password
                });
                await _userContext.SaveChangesAsync();
                return Ok();
            }
            
        }
        return BadRequest("user was registered");
    }

    [HttpPost, AllowAnonymous]
    public IActionResult CreateToken([FromBody] UserView user)
    {
        if (user.UserName != null && user.Password != null)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes
                (_configuration["Jwt:Key"]);
            var expireInMinutes = _configuration["Jwt:Expire"];
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return Ok(stringToken);
        }

        return Unauthorized();
    }
}