using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shop.Api.Models.ViewModels;
using Shop.Application;
using Shop.Application.Services;
using Shop.Data.DbContexts;
using Shop.Domain.Models;

namespace Shop.Api.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class UserController:ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserService _userService;

    public UserController(IConfiguration configuration, ApplicationDbContext context, 
        SignInManager<User> signInManager, UserManager<User>userManager, RoleManager<IdentityRole> roleManager,
        UserService userService)
    {
        _configuration = configuration;
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _userService = userService;
    }
 
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserView userView)
    {
        if (userView.UserName != String.Empty && userView.Password != String.Empty)
        {
            var userInDb = await _userManager.FindByNameAsync(userView.UserName);
            if (userInDb==null)
            {
                var identityResult = await _userManager.CreateAsync(new User()
                {
                    UserName = userView.UserName,
                    Email = userView.Email,
                    EmailConfirmed = userView.EmailConfirmed
                }, userView.Password);
                if (identityResult.Succeeded)
                {
                    userInDb = await _userManager.FindByNameAsync(userView.UserName);
                    if(await _userService.IsAddUserToRole(userInDb, new []{"user"}))
                        return Ok();
                }
                return BadRequest(new {message= "data isn't correct"});
                
            }
            
        }
        return BadRequest(new {message= "user was registered"});
    }
    
    [HttpPost]
    public async Task<IActionResult> RegisterAdmin([FromBody] UserView userView)
    {
        if (userView.UserName != String.Empty && userView.Password != String.Empty)
        {
            var userInDb = await _userManager.FindByNameAsync(userView.UserName);
            if (userInDb==null)
            {
                var identityResult = await _userManager.CreateAsync(new User()
                {
                    UserName = userView.UserName,
                    Email = userView.Email,
                    EmailConfirmed = userView.EmailConfirmed
                }, userView.Password);
                if (identityResult.Succeeded)
                {
                    userInDb = await _userManager.FindByNameAsync(userView.UserName);
                    if(await _userService.IsAddUserToRole(userInDb, new []{"user", "admin"}))
                        return Ok();
                }
                return BadRequest(new {message= "data isn't correct"});
                
            }
            
        }
        return BadRequest(new {message= "user was registered"});
    }
}