namespace E_CommerceApi.DTOs.Order;

public record OrderRequestDto(
    List<OrderItemRequestDto> Items
    );