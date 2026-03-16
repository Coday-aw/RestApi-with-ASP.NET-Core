namespace E_CommerceApi.DTOs.Product;

public record ProductResponseDto(
     int Id,
     string Name,
     decimal Price,
     string Description,
     string Category,
     int Stock
);