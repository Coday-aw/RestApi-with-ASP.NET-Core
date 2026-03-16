namespace E_CommerceApi.DTOs.Order;
using System.ComponentModel.DataAnnotations;

public record OrderItemRequestDto
(
    [Required(ErrorMessage = "Product Id is required.")]
    int ProductId,
    
    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    int Quantity
);