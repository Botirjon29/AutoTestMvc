namespace AutoTest.WebMvc.Services;

public class CookiesServices
{
    public string? GetUserFromPhoneCookie(HttpContext context)
    {
        if (context.Request.Cookies.ContainsKey("UserPhone"))
        {
            return context.Request.Cookies["UserPhone"];
        }
        return null;
    }

    public void SendUserPhoneToCookie(string userPhone, HttpContext context)
    {
        var options = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(1)
        };
        context.Response.Cookies.Append("UserPhone", userPhone, options);
    }

    public void UdateUserPhoneToCookie(string userPhone, HttpContext context)
    {
        context.Response.Cookies.Delete("UserPhone");

        var options = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(1)
        };
        context.Response.Cookies.Append("UserPhone", userPhone, options);
    }
}
