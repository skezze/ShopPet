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
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }
    [AllowAnonymous,HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(_context.Products);
    }

    [AllowAnonymous,HttpGet("{id}")]
    public async Task<IActionResult> GetProductById( int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product!=null)
        {
            return Ok(product);   
        }

        return NotFound();
    }

    [Authorize(Policy = "Admin"),HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody]ProductView productView)
    {
       if (productView.Title!=""&&productView.Description!=""&&productView.Price>0)
        {
            var product = await _context.Products.AddAsync(new Product()
            {
                Title = productView.Title,
                Description = productView.Description,
                Price = productView.Price,
                });
            await _context.SaveChangesAsync();
            return Ok(product.Entity);
        }
        
        return BadRequest();
    }

    [HttpDelete, Authorize(Policy = "Admin"), HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product!=null)
        {
            _context.Remove(product);
            await _context.SaveChangesAsync();
        }

        return Ok();
    }
    
    [Authorize(Policy = "Admin"),HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody]Product product)
    {
        if (product.Id>0&&product.Title!=""&&product.Description!=""&&product.Price>0)
        {
            var productInDb = await _context.Products.FirstOrDefaultAsync(x=>x.Id==product.Id);
            if (productInDb!=null)
            {
                productInDb.Description = product.Description;
                productInDb.Title = product.Title;
                productInDb.Price = product.Price;
                await _context.SaveChangesAsync();
                return Ok(productInDb);
            }
           
        }
        
        return BadRequest();
    }
    
}