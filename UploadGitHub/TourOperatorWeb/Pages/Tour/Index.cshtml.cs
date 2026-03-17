using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.Tour;

public class IndexModel : PageModel
{
    private readonly UngDungDbContext _db;

    public IndexModel(UngDungDbContext db)
    {
        _db = db;
    }

    [BindProperty(SupportsGet = true)]
    public string? TuKhoa { get; set; }

    public List<TourDuLich> DanhSachTour { get; set; } = new();

    public async Task OnGetAsync()
    {
        var truyVan = _db.TourDuLichs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(TuKhoa))
        {
            var tuKhoa = TuKhoa.Trim();

            truyVan = truyVan.Where(t =>
                t.TenTour.Contains(tuKhoa) ||
                t.DiaDiemKhoiHanh.Contains(tuKhoa) ||
                t.DiaDiemDen.Contains(tuKhoa));
        }

        DanhSachTour = await truyVan
            .OrderByDescending(t => t.LaDeXuat)
            .ThenBy(t => t.TenTour)
            .ToListAsync();
    }
}