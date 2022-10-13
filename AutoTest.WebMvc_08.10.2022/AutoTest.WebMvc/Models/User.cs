using AutoTest.WebMvc.Validations;
using System.ComponentModel.DataAnnotations;
using PhoneAttribute = AutoTest.WebMvc.Validations.PhoneAttribute;

namespace AutoTest.WebMvc.Models;

public class User
{
    public int Id { get; set; }

    [Required]

    [StringLength(20, MinimumLength = 5)]
    public string? Name { get; set; }

    [Phone]
    public string? Phone { get; set; }

    [Password(4)]
    public string? Password { get; set; }
    public string? Image { get; set; }

    public IFormFile? ImageFile { get; set; }
}
