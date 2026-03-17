using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class Xe_XoaModel : PageModel
{
    private readonly UngDungDbContext _db;

    public Xe_XoaModel(UngDungDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public int Id { get; set; }

    public string? BienSo { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var xe = await _db.Xes.FirstOrDefaultAsync(x => x.Id == id);
        if (xe == null)
        {
            BienSo = null;
        }
        else
        {
            Id = xe.Id;
            BienSo = xe.BienSo;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var xe = await _db.Xes.FirstOrDefaultAsync(x => x.Id == Id);
        if (xe != null)
        {
            _db.Xes.Remove(xe);
            await _db.SaveChangesAsync();
        }

        // Khi mở trong iframe (modal), trả về script yêu cầu trang cha reload để cập nhật danh sách
        return Content("<script>window.parent.location.reload();</script>", "text/html");
    }
}
