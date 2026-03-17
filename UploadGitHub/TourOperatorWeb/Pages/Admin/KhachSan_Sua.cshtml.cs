using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;
using KhachSanEntity = TourOperatorWeb.Models.KhachSan;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class KhachSan_SuaModel : PageModel
{
    private readonly UngDungDbContext _db;

    public KhachSan_SuaModel(UngDungDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public KhachSanEntity KhachSan { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var ks = await _db.KhachSans.FirstOrDefaultAsync(k => k.Id == id);
        if (ks == null)
        {
            return RedirectToPage("/Admin/QuanLyKhachSan");
        }

        KhachSan = ks;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var ks = await _db.KhachSans.FirstOrDefaultAsync(k => k.Id == KhachSan.Id);
        if (ks == null)
        {
            return RedirectToPage("/Admin/QuanLyKhachSan");
        }

        ks.TenKhachSan = KhachSan.TenKhachSan;
        ks.DiaChi = KhachSan.DiaChi;
        ks.SoDienThoai = KhachSan.SoDienThoai;

        await _db.SaveChangesAsync();

        // Khi mở trong iframe (modal), trả về script yêu cầu trang cha reload để cập nhật danh sách
        return Content("<script>window.parent.location.reload();</script>", "text/html");
    }
}
