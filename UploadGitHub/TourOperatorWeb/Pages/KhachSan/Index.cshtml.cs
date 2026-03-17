using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;
using KhachSanEntity = TourOperatorWeb.Models.KhachSan;

namespace TourOperatorWeb.Pages.KhachSanPages;

public class IndexModel : PageModel
{
    private readonly UngDungDbContext _db;

    public IndexModel(UngDungDbContext db)
    {
        _db = db;
    }

    public IList<KhachSanEntity> DanhSachKhachSan { get; set; } = new List<KhachSanEntity>();

    public async Task OnGetAsync()
    {
        DanhSachKhachSan = await _db.KhachSans
            .OrderBy(k => k.TenKhachSan)
            .ToListAsync();
    }
}
