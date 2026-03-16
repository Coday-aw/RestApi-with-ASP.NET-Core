using System.Security.Claims;
using E_CommerceApi.DTOs.Order;
using E_CommerceApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceApi.Controllers;

public class OrderController : ControllerBase
{
    private readonly IOrderService  _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> CreateOrderAsync(OrderRequestDto orderRequest)
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var response = await _orderService.CreateOrder(orderRequest, userId);
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrdersAsync()
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var response = await _orderService.GetOrders(userId);
        return Ok(response);
    }
}