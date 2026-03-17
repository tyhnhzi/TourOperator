using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class TrangChuAdminModel : PageModel
{
    public void OnGet()
    {
    }
}
