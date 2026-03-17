using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class Xe_SuaModel : PageModel
{
    private readonly UngDungDbContext _db;

    public Xe_SuaModel(UngDungDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public Xe Xe { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var xe = await _db.Xes.FirstOrDefaultAsync(x => x.Id == id);
        if (xe == null)
        {
            return RedirectToPage("/Admin/QuanLyXe");
        }

        Xe = xe;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var xe = await _db.Xes.FirstOrDefaultAsync(x => x.Id == Xe.Id);
        if (xe == null)
        {
            return RedirectToPage("/Admin/QuanLyXe");
        }

        xe.BienSo = Xe.BienSo;
        xe.LoaiXe = Xe.LoaiXe;
        xe.SoChoNgoi = Xe.SoChoNgoi;
        xe.TenTaiXe = Xe.TenTaiXe;

        await _db.SaveChangesAsync();

        // Khi mở trong iframe (modal), trả về script yêu cầu trang cha reload để cập nhật danh sách
        return Content("<script>window.parent.location.reload();</script>", "text/html");
    }
}
