using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.Controllers;
[ApiController, Route("/[controller]/[action]"),Authorize]
public class HomeController:ControllerBase
{
    [HttpGet]
    public IActionResult GetHello()
    {
        return Ok("hello");
    }
}