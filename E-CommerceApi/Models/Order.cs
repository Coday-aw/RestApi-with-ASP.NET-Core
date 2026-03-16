namespace E_CommerceApi.Models;

public class Order
{
    public int Id {get; set;}
    public DateTime OrderDate {get; set;}
    public string UserId { get; set; }
    public User User {get; set;}
    public List<OrderItem> Items {get; set;} = new List<OrderItem>();
    public IEnumerable<OrderItem> OrderItem { get; set; }
}