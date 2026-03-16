namespace E_CommerceApi.DTOs.Customer;
using System.ComponentModel.DataAnnotations;
public record UserUpdateDto
(
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be at least 2 characters long.")]
    string FirstName,

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be at least 2 characters long.")]
    string LastName
);