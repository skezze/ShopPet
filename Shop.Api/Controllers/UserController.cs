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
    private readonly ApplicationDbContext _context;

    public UserController(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }
 
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserView userView)
    {
        if (userView.UserName != String.Empty && userView.Password != String.Empty)
        {
            var userInDb = await _context.Users.FirstOrDefaultAsync(
                u => u.UserName == userView.UserName && u.Password == userView.Password);
            if (userInDb==null)
            {
                await _context.Users.AddAsync(new User()
                {
                    UserName = userView.UserName,
                    Password = userView.Password,
                    Role = Role.User
                });
                await _context.SaveChangesAsync();
                return Ok();
            }
            
        }
        return BadRequest(new {message= "user was registered"});
    }
    
    [HttpPost, Authorize(Policy = "Admin")]
    public async Task<IActionResult> RegisterForAdmin([FromBody] UserViewAdmin userView)
    {
        if (userView.UserName != String.Empty && userView.Password != String.Empty)
        {
            var userInDb = await _context.Users.FirstOrDefaultAsync(
                u => u.UserName == userView.UserName && u.Password == userView.Password);
            if (userInDb==null)
            {
                await _context.Users.AddAsync(new User()
                {
                    UserName = userView.UserName,
                    Password = userView.Password,
                    Role = userView.Role
                });
                await _context.SaveChangesAsync();
                return Ok();
            }
            
        }
        return BadRequest(new {message= "user was registered"});
    }

    [HttpPost, AllowAnonymous]
    public IActionResult CreateToken([FromBody] UserView userView)
    {
        if (userView.UserName != String.Empty && userView.Password != String.Empty)
        {
           var userInDb = _context.Users.FirstOrDefault(x => x.UserName == userView.UserName && x.Password == userView.Password);
           if (userInDb == null)
           {
               return NotFound();
           }
           
           
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes
                (_configuration["Jwt:Key"]);
            var expireInMinutes = _configuration["Jwt:Expire"];
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", userInDb.Id.ToString()),
                    new Claim("UserName", userInDb.UserName),
                    new Claim("Role", userInDb.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(expireInMinutes)),
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
            return Ok(new { token = stringToken});
        }

        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> AdminRegister([FromBody] UserView userView)
    {
        if (userView.UserName != string.Empty && userView.Password != string.Empty)
        {
            var userInDb = await _context.Users.FirstOrDefaultAsync(
                u => u.UserName == userView.UserName && u.Password == userView.Password);
            if (userInDb==null)
            {
                await _context.Users.AddAsync(new User()
                {
                    UserName = userView.UserName,
                    Password = userView.Password,
                    Role = Role.Admin
                });
                await _context.SaveChangesAsync();
                return Ok();
            }
            
        }
        return BadRequest(new{message = "user was registered"});
    }
}