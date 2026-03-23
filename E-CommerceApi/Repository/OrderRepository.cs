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
        // Get all ids of all product from order.items
        var productsIds = order.Items.Select(i => i.ProductId).ToList();
        
        // Get the products that contains the id from productsIds
        var products = await _context.Products.Where(p => productsIds.Contains(p.Id)).ToListAsync();

        // Convert to Dictionary for faster loopup 
        var productDictionary = products.ToDictionary(p => p.Id);
        
        // Loop all products 
        foreach (var item in order.Items)
        {
            // if product exist 
            if(!productDictionary.TryGetValue(item.ProductId, out var product))
                throw new NotFoundException($"product with id {item.Id} not found");
            // check if product quantity is in stock 
            if (product.Stock < item.Quantity)
                throw new InsufficientException($"Not enough stock for product with id: {item.Id}");
        }
        // Loop all items in order and decrease stock 
        foreach (var item in order.Items)
        {
            var product =  productDictionary[item.ProductId];
            product.Stock -= item.Quantity;
        }
        // Create order 
        var newOrder = new Order
        {
            OrderDate = order.OrderDate,
            UserId = order.UserId,
            Items = order.Items,
        };
        // Add order and save changes and return newOrder
        await _context.Orders.AddAsync(newOrder);
        await _context.SaveChangesAsync(); 
        return newOrder;
    }
}