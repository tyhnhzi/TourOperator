using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class Tour_XoaModel : PageModel
{
    private readonly UngDungDbContext _nganHangDuLieu;

    public Tour_XoaModel(UngDungDbContext nganHangDuLieu)
    {
        _nganHangDuLieu = nganHangDuLieu;
    }

    [BindProperty]
    public int Id { get; set; }

    public TourDuLich? Tour { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var tour = await _nganHangDuLieu.TourDuLichs.FirstOrDefaultAsync(t => t.Id == id);
        if (tour == null)
        {
            return RedirectToPage("/Admin/QuanLyTour");
        }

        Id = id;
        Tour = tour;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var tour = await _nganHangDuLieu.TourDuLichs.FirstOrDefaultAsync(t => t.Id == Id);
        if (tour != null)
        {
            _nganHangDuLieu.TourDuLichs.Remove(tour);
            await _nganHangDuLieu.SaveChangesAsync();
        }

        // Khi mở trong iframe (modal), trả về script yêu cầu trang cha reload để cập nhật danh sách
        return Content("<script>window.parent.location.reload();</script>", "text/html");
    }
}
