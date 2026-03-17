using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class QuanLyXeModel : PageModel
{
    private readonly UngDungDbContext _db;

    public QuanLyXeModel(UngDungDbContext db)
    {
        _db = db;
    }

    public List<Xe> DanhSachXe { get; set; } = new();

    public async Task OnGetAsync()
    {
        DanhSachXe = await _db.Xes
            .OrderBy(x => x.BienSo)
            .ToListAsync();
    }
}
