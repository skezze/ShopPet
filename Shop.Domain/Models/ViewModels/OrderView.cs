namespace Shop.Api.Models.ViewModels;

public class OrderView
{
    public ICollection<int> ProductIds { get; set; }
    public string Adress1 { get; set; }
    public string Address2 { get; set; }
    public int PostCode { get; set; }
    public string UserId { get; set; }
}