using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class Xe_ThemModel : PageModel
{
    private readonly UngDungDbContext _db;

    public Xe_ThemModel(UngDungDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public Xe Xe { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _db.Xes.Add(Xe);
        await _db.SaveChangesAsync();

        // Khi mở trong iframe (modal), trả về script yêu cầu trang cha reload để thấy xe mới
        return Content("<script>window.parent.location.reload();</script>", "text/html");
    }
}
