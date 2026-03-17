using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TourOperatorWeb.Pages;

public class DoiNgonNguModel : PageModel
{
    public IActionResult OnGet(string lang = "vi", string? returnUrl = null)
    {
        if (lang != "en")
        {
            lang = "vi";
        }

        Response.Cookies.Append("ngonngu", lang, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            IsEssential = true
        });

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToPage("/Index");
    }
}
