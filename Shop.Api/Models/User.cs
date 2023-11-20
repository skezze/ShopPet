using Shop.Api.Models.ViewModels;

namespace Shop.Api.Models;

public class User:UserView
{
    public int Id {get; set;}
    public Role Role { get; set; }

}