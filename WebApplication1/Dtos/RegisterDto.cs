using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public record class RegisterDto(
    [Required] string Email,
    [Required] string Password,
    [Required] string Role 
);
