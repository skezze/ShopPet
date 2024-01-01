using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.ViewModels;
using Shop.Application.Services;
using Shop.Data.DbContexts;
using Shop.Domain.Models;

namespace Shop.Api.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class UserController:ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly UserService _userService;
    public UserController(UserManager<User>userManager, UserService userService, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
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
                var registeredUser = await _userService.Register(userInDb);
                if (registeredUser!=null)
                {
                    var result = await _userService.AddUserToRole(registeredUser, new string[]{"user"});
                    if (result)
                    {
                        return Ok(registeredUser);
                    }
                }

                return BadRequest(new { message = "register is failed" });
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
                var registeredUser = await _userService.Register(userInDb);
                if (registeredUser!=null)
                {
                    var result = await _userService.AddUserToRole(registeredUser, new string[]{"user","admin"});
                    if (result)
                    {
                        return Ok(registeredUser);
                    }
                }

                return BadRequest(new { message = "register is failed" });
            }
        }
        return BadRequest(new {message= "user was registered"});
    }
}