using E_CommerceApi.DTOs.Order;
using E_CommerceApi.Interfaces;
using E_CommerceApi.Models;

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
       var orders = await _orderRepository.GetOrdersByUserAsync(userId);
       
       if (!orders.Any())
           return new List<OrderResponseDto>();

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
        
       var newOrder = await _orderRepository.CreateOrderAsync(order);

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