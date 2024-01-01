using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Models.ViewModels;
using Shop.Data.DbContexts;
using Shop.Domain.Models;

namespace Shop.Api.Controllers;
[ApiController,Route("api/[controller]/[action]"),Authorize]
public class OrderController:ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetOrders()
    {
        
        return Ok(_context.Orders);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        if (id > 0)
        {
           var order = await _context.Orders
               .Include(x=>x.User)
               .Include(x=>x.OrderProducts)
               .FirstOrDefaultAsync(x => x.Id == id);
           if (order!=null)
           {
               return Ok(order);
           }
           
        }

        return BadRequest(new {message = "it is not valid id, or order is not exists"});

    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody]OrderView orderView)
    {
        if (orderView.UserId!=string.Empty && orderView.ProductIds.Count>0 && orderView.Address2!= string.Empty
            && orderView.Adress1!= string.Empty && orderView.PostCode>0)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == orderView.UserId);
            
            if (user==null)
            {
                return BadRequest(new {message = "user not exist"});
            }
            
            var finalPrice = async () =>
            {
                int price = 0;
                foreach (var productId in orderView.ProductIds)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
                    if (product!=null)
                    {
                        price += product.Price;
                    }
                }

                return price;
            };
            var orderproducts = async () =>
            {
                var orderProducts = new List<OrderProduct>();
                foreach (var productId in orderView.ProductIds)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
                    if (product != null)
                    {
                        var orderProduct = new OrderProduct
                        {
                                  // order - ваш созданный заказ
                            Product = product   // текущий продукт
                        };
                        orderProducts.Add(orderProduct);
                    }
                }
                return orderProducts;
            };

            var order = new Order()
            {
                User = user,
                Address2 = orderView.Address2,
                Adress1 = orderView.Adress1,
                FinalPrice = await finalPrice(),
                PostCode = orderView.PostCode,
                OrderProducts = await orderproducts(),
                Status = OrderStatus.InProcess
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return Ok(order);
        }

        return BadRequest(new { message = "Order data isn't valid" });
    }

    [HttpPut, Authorize(Policy = "Admin"),Route("{orderId}/{orderStatus}")]
    public async Task<IActionResult> UpdateOrderStatus( int orderId, OrderStatus orderStatus)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        if (order==null)
        {
            return BadRequest(new {message = "order isn`t exist"});
        }

        if (!Enum.IsDefined(typeof(OrderStatus),orderStatus))
        {
            return BadRequest(new {message = "order status isn`t exist"});
        }

        order.Status = orderStatus;
        await _context.SaveChangesAsync();
        return Ok(order);
    }

    [HttpDelete, Authorize(Policy = "Admin"), Route("{orderId}")]
    public async Task<IActionResult> Delete(int orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        if (order==null)
        {
            return BadRequest(new {message = "order isn`t exist"});
        }

        _context.Orders.Remove(order);
        return Ok(new {message = "removed"});
    }
}