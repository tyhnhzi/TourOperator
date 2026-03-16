using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;
using KhachSanEntity = TourOperatorWeb.Models.KhachSan;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class QuanLyKhachSanModel : PageModel
{
    private readonly UngDungDbContext _db;

    public QuanLyKhachSanModel(UngDungDbContext db)
    {
        _db = db;
    }

    public List<KhachSanEntity> DanhSachKhachSan { get; set; } = new();

    public async Task OnGetAsync()
    {
        DanhSachKhachSan = await _db.KhachSans
            .OrderBy(k => k.TenKhachSan)
            .ToListAsync();
    }
}
