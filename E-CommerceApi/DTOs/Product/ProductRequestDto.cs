namespace E_CommerceApi.DTOs.Product;
using System.ComponentModel.DataAnnotations;

public record ProductRequestDto
(
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
     string Name,
    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    decimal Price,
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 500 characters.")]
    string Description,
    [Required(ErrorMessage = "Category is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Category must be between 1 and 50 characters.")]
    string Category,
    [Required(ErrorMessage = "Stock is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")] 
    int Stock
);