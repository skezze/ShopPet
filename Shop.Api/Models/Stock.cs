using System.Security.Principal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Shop.Api.Models;

public class Stock
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }

    public int ProductId { get; set; }
    public Product Products { get; set; }
}