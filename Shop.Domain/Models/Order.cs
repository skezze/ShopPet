using Shop.Api.Models;

namespace Shop.Domain.Models;

public class Order
{
    public int Id { get; set; }
    public string Adress1 { get; set; }
    public string Address2 { get; set; }
    public int PostCode { get; set; }
    public int FinalPrice { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public OrderStatus Status { get; set; }
    
    public ICollection<OrderProduct> OrderProducts { get; set; }
}