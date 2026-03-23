using Microsoft.AspNetCore.Identity;

namespace E_CommerceApi.Models;

public class User : IdentityUser
{
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    public ICollection<Order> Orders {get; set;} = new List<Order>();
}