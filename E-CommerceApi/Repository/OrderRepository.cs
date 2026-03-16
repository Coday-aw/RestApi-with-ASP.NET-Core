using E_CommerceApi.Data;
using E_CommerceApi.DTOs.Order;
using E_CommerceApi.Exceptions;
using E_CommerceApi.Interfaces;
using E_CommerceApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceApi.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId)
    {
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .ThenInclude(itm => itm.Product).ToListAsync();
        return orders;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        var newOrder = new Order
        {
            OrderDate = order.OrderDate,
            UserId = order.UserId,
            Items = order.Items,
        };

        await _context.Orders.AddAsync(newOrder);
        await _context.SaveChangesAsync();
        return newOrder;
    }
}