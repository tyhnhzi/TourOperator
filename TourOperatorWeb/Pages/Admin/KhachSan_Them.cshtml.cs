using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;
using KhachSanEntity = TourOperatorWeb.Models.KhachSan;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class KhachSan_ThemModel : PageModel
{
    private readonly UngDungDbContext _db;

    public KhachSan_ThemModel(UngDungDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public KhachSanEntity KhachSan { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _db.KhachSans.Add(KhachSan);
        await _db.SaveChangesAsync();

        // Khi mở trong iframe (modal), trả về script yêu cầu trang cha reload để thấy khách sạn mới
        return Content("<script>window.parent.location.reload();</script>", "text/html");
    }
}
