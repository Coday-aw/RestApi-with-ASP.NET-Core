namespace E_CommerceApi.DTOs.Order;

public record OrderItemResponseDto
(
    int ProductId,
    string ProductName,
    decimal productPrice,
    int Quantity,
    decimal Price);