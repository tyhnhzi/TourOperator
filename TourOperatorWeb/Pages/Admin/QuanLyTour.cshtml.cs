using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class QuanLyTourModel : PageModel
{
    private readonly UngDungDbContext _nganHangDuLieu;

    public QuanLyTourModel(UngDungDbContext nganHangDuLieu)
    {
        _nganHangDuLieu = nganHangDuLieu;
    }

    public List<TourDuLich> DanhSachTour { get; set; } = new();

    public async Task OnGetAsync()
    {
        DanhSachTour = await _nganHangDuLieu.TourDuLichs
            .OrderBy(t => t.TenTour)
            .ToListAsync();
    }
}
