using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class BaoCaoLoiNhuanTourModel : PageModel
{
    private readonly UngDungDbContext _nganHangDuLieu;

    public BaoCaoLoiNhuanTourModel(UngDungDbContext nganHangDuLieu)
    {
        _nganHangDuLieu = nganHangDuLieu;
    }

    public List<TourLoiNhuanViewModel> DanhSachBaoCao { get; set; } = new();

    public class TourLoiNhuanViewModel
    {
        public int TourId { get; set; }
        public string TenTour { get; set; } = string.Empty;
        public decimal TongThu { get; set; }
        public decimal TongChi { get; set; }
        public decimal LoiNhuanRong => TongThu - TongChi;
    }

    public async Task OnGetAsync()
    {
        var tours = await _nganHangDuLieu.TourDuLichs
            .Include(t => t.DoanKhachs)
            .Include(t => t.ChiPhis)
            .ToListAsync();

        DanhSachBaoCao = tours
            .Select(t => new TourLoiNhuanViewModel
            {
                TourId = t.Id,
                TenTour = t.TenTour,
                TongThu = t.DoanKhachs.Sum(d => d.SoKhach * d.DonGiaMotKhach),
                TongChi = t.ChiPhis.Sum(c => c.SoTien)
            })
            .OrderByDescending(b => b.LoiNhuanRong)
            .ToList();
    }
}
