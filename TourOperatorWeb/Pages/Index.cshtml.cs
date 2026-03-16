using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages;

public class IndexModel : PageModel
{
    private readonly UngDungDbContext _nganHangDuLieu;

    public IndexModel(UngDungDbContext nganHangDuLieu)
    {
        _nganHangDuLieu = nganHangDuLieu;
    }

    public List<TourDuLich> DanhSachTourDeXuat { get; set; } = new();

    public async Task OnGetAsync()
    {
        var truyVan = _nganHangDuLieu.TourDuLichs
            .Where(t => t.LaDeXuat);

        // SQLite khong ho tro sap xep truc tiep theo decimal trong ORDER BY,
        // nen lay ve bo nho roi sap xep lai tren client.
        DanhSachTourDeXuat = (await truyVan.ToListAsync())
            .OrderBy(t => t.GiaTu)
            .ToList();
    }
}
