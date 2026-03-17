using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.ThueXe;

public class IndexModel : PageModel
{
    private readonly UngDungDbContext _db;

    public IndexModel(UngDungDbContext db)
    {
        _db = db;
    }

    public IList<Xe> DanhSachXe { get; set; } = new List<Xe>();

    public async Task OnGetAsync()
    {
        DanhSachXe = await _db.Xes
            .OrderBy(x => x.LoaiXe)
            .ThenBy(x => x.BienSo)
            .ToListAsync();
    }
}
