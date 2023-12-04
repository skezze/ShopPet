namespace Shop.Api.Models;

public class Order
{
    public int Id { get; set; }
    public ICollection<Product> Products { get; set; }
    public string Adress1 { get; set; }
    public string Address2 { get; set; }
    public int PostCode { get; set; }
    public int FinalPrice { get; set; }
}