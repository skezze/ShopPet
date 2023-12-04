using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Shop.Api.Data;
using Shop.Api.Models;
using Shop.Api.Models.ViewModels;

namespace Shop.Api.Controllers;
[ApiController, Route("api/[controller]/[action]")]
public class ProductController:ControllerBase
{
    private readonly UserDbContext _userDbContext;
    private readonly ApplicationDbContext _applicationDbContext;

    public ProductController(UserDbContext userDbContext, ApplicationDbContext applicationDbContext)
    {
        _userDbContext = userDbContext;
        _applicationDbContext = applicationDbContext;
    }
    [AllowAnonymous,HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(_applicationDbContext.Products);
    }

    [AllowAnonymous,HttpGet("{id}")]
    public async Task<IActionResult> GetProductById( int id)
    {
        var product = await _applicationDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product!=null)
        {
            return Ok(product);   
        }

        return NotFound();
    }

    [Authorize(Policy = "Admin"),HttpPost("{title}/{description}/{price}")]
    public async Task<IActionResult> CreateProduct(string title, string description, int price)
    {
       if (title!=""&&description!=""&&price>0)
        {
            var product = await _applicationDbContext.Products.AddAsync(new Product()
            {
                Title = title,
                Description = description,
                Price = price,
                });
            await _applicationDbContext.SaveChangesAsync();
            return Ok(product.Entity);
        }
        
        return BadRequest();
    }

    [HttpDelete, Authorize(Policy = "Admin"), HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _applicationDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product!=null)
        {
            _applicationDbContext.Remove(product);
            await _applicationDbContext.SaveChangesAsync();
        }

        return Ok();
    }
    
    [Authorize(Policy = "Admin"),HttpPut("{id}/{title}/{description}/{price}")]
    public async Task<IActionResult> UpdateProduct(int id, string title, string description, int price)
    {
        if (id>0&&title!=""&&description!=""&&price>0)
        {
            var product = await _applicationDbContext.Products.FirstOrDefaultAsync(x=>x.Id==id);
            if (product!=null)
            {
                product.Description = description;
                product.Title = title;
                product.Price = price;
                await _applicationDbContext.SaveChangesAsync();
                return Ok(product);
            }
           
        }
        
        return BadRequest();
    }
    
}