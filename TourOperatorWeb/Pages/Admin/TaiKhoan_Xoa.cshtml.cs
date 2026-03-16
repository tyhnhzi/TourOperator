using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class TaiKhoan_XoaModel : PageModel
{
    private readonly UngDungDbContext _db;

    public TaiKhoan_XoaModel(UngDungDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public int Id { get; set; }

    public string? TenDangNhap { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var nguoiDung = await _db.NguoiDungs.FirstOrDefaultAsync(n => n.Id == id);
        if (nguoiDung == null)
        {
            TenDangNhap = null;
        }
        else
        {
            Id = nguoiDung.Id;
            TenDangNhap = nguoiDung.TenDangNhap;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var nguoiDung = await _db.NguoiDungs.FirstOrDefaultAsync(n => n.Id == Id);
        if (nguoiDung != null)
        {
            _db.NguoiDungs.Remove(nguoiDung);
            await _db.SaveChangesAsync();
        }

        // Khi mở trong iframe (modal), trả về script yêu cầu trang cha reload để cập nhật danh sách
        return Content("<script>window.parent.location.reload();</script>", "text/html");
    }
}
