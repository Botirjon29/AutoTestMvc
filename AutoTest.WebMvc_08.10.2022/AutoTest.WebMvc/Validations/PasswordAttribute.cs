
using System.ComponentModel.DataAnnotations;

namespace AutoTest.WebMvc.Validations;

public class PasswordAttribute : ValidationAttribute
{
    public int MinLength = 4;
    public PasswordAttribute(int minL)
    {
            MinLength = minL;
    }
    public override bool IsValid(object? value)
    {
        var _value = (string)value;
        if (_value == null)
        {
            ErrorMessage = "Password is null";
        }
        else if (_value.Length <= MinLength)
        {
            ErrorMessage = $"Password must be minimum length of {MinLength}";
        }

        return !string.IsNullOrEmpty(_value) && _value.Length >= MinLength;
    }
}
