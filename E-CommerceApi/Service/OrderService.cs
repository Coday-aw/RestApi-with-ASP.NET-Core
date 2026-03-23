using E_CommerceApi.Data;
using E_CommerceApi.DTOs.Order;
using E_CommerceApi.Exceptions;
using E_CommerceApi.Interfaces;
using E_CommerceApi.Models;
using Microsoft.AspNetCore.Identity;

namespace E_CommerceApi.Service;

public class OrderService : IOrderService
{
    
    private readonly IOrderRepository _orderRepository;
    
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrders(string userId)
    {
        // 1. Get order from order repository
        var orders = await _orderRepository.GetOrdersByUserAsync(userId);
       
        // 2. if user dont have orders send back empty dto
       if (!orders.Any())
           return new List<OrderResponseDto>();
       
       // 3. if user have orders send back a list of the orders
       return orders.Select(o => new OrderResponseDto(
           o.Id,
           o.OrderDate,
           o.UserId,
           o.Items.Select(itm => new OrderItemResponseDto(
               itm.ProductId,
               itm.Product.Name,
               itm.Product.Price,
               itm.Quantity,
               itm.Price))
       )).ToList();
    }

    public async Task<OrderResponseDto> CreateOrder(OrderRequestDto orderRequestDto, string userId)
    {
        // 1. Put values from dto to create a new order
        var order = new Order
        {
            OrderDate = DateTime.Now,
            UserId = userId,
            Items = orderRequestDto.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };
        
        // 2. Send the new order to order repository put the order in the database
       var newOrder = await _orderRepository.CreateOrderAsync(order);
       
       // 3. Create dto response and send back to client
       return new OrderResponseDto(
           newOrder.Id,
           newOrder.OrderDate,
           newOrder.UserId,
           newOrder.Items.Select(i => new OrderItemResponseDto(
               i.ProductId,
               i.Product.Name,
               i.Product.Price,
               i.Quantity,
               i.Price))
       );
    }
}