using E_CommerceApi.DTOs.Order;
using E_CommerceApi.Models;

namespace E_CommerceApi.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId);
    Task<Order> CreateOrderAsync(Order order); 
}