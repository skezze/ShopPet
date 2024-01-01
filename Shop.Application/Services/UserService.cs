using Microsoft.AspNetCore.Identity;
using Shop.Data.DbContexts;
using Shop.Domain.Models;

namespace Shop.Application.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(ApplicationDbContext context, SignInManager<User> signInManager, 
        UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> IsLogin(User user)
    {
        var id = await _userManager.GetUserIdAsync(user);
        if (id==string.Empty)
        {
            return false;
        }
        await _signInManager.SignInAsync(user, isPersistent: true);
        return true;
    }

    public async Task<bool> IsRegister(User user)
    {
        var id = await _userManager.GetUserIdAsync(user);
        if (id!=string.Empty)
        {
            return false;
        }
        await _signInManager.SignInAsync(user, isPersistent: true);
        return true;
    }
    public async Task<bool> IsAddUserToRole(User user,string[] roleNames)
    {
        var result = await _userManager.AddToRolesAsync(user, roleNames);
        if (result.Succeeded)
        {
            return true;
        }

        return false;
    }
   
    
}