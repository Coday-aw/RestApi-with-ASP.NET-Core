using E_CommerceApi.DTOs.Order;
using E_CommerceApi.Models;

namespace E_CommerceApi.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderResponseDto>> GetOrders(string userId);
    Task<OrderResponseDto> CreateOrder(OrderRequestDto orderRequestDto,  string userId); 
}