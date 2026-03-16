namespace E_CommerceApi.DTOs.Order;

public record OrderResponseDto
(
    int Id,
    DateTime OrderDate,
    string UserId,
    IEnumerable<OrderItemResponseDto> Items
);