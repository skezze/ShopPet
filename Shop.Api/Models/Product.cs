using Shop.Api.Models.ViewModels;

namespace Shop.Api.Models;

public class Product:ProductView
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }

}