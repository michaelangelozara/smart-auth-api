namespace SmartAuth.WebAPI.Extensions;

public static class HttpResponseExtensions
{
    public static void SetCookie(this HttpResponse response, string name, string value, int expirationInHours)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddHours(expirationInHours),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/"
        };

        response.Cookies.Append(name, value, cookieOptions);
    }
}