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
        context.Response.Cookies.Append("UserPhone", userPhone);
    }
}
