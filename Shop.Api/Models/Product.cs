namespace Shop.Api.Models;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }

    public ICollection<Stock> Stocks { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; }
}