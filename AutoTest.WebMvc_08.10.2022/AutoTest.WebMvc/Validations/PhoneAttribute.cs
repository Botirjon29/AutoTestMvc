using System.ComponentModel.DataAnnotations;

namespace AutoTest.WebMvc.Validations;

public class PhoneAttribute : ValidationAttribute
{
    public int MinLength = 9;
    public override bool IsValid(object? value)
    {
        var _value = (string)value;

        if (_value == null)
        {
            ErrorMessage = "Phone is null";
        }
        else if (_value.Length < MinLength)
        {
            ErrorMessage = $"Phone must be minimum length of {MinLength}";
        }
        else
        {
            var isNumber = long.TryParse(_value, out _);
            if (!isNumber)
            {
                ErrorMessage = "Phone is not valid";
            return false;
            }
        }

        return !string.IsNullOrEmpty(_value) && _value.Length >= MinLength;
    }
}
