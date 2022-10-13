using AutoTest.WebMvc.Validations;

namespace AutoTest.WebMvc.Models
{
    public class UserDto
    {
        [Phone]
        public string? Phone { get; set; }

        [Password(4)]
        public string? Password { get; set; }
    }
}
