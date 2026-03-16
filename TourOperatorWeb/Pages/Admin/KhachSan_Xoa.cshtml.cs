using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class KhachSan_XoaModel : PageModel
{
    private readonly UngDungDbContext _db;

    public KhachSan_XoaModel(UngDungDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public int Id { get; set; }

    public string? TenKhachSan { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var ks = await _db.KhachSans.FirstOrDefaultAsync(k => k.Id == id);
        if (ks == null)
        {
            TenKhachSan = null;
        }
        else
        {
            Id = ks.Id;
            TenKhachSan = ks.TenKhachSan;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var ks = await _db.KhachSans.FirstOrDefaultAsync(k => k.Id == Id);
        if (ks != null)
        {
            _db.KhachSans.Remove(ks);
            await _db.SaveChangesAsync();
        }

        // Khi mở trong iframe (modal), trả về script yêu cầu trang cha reload để cập nhật danh sách
        return Content("<script>window.parent.location.reload();</script>", "text/html");
    }
}
