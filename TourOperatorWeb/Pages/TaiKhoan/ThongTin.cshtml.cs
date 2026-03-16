using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.TaiKhoan;

public class ThongTinModel : PageModel
{
    private readonly UngDungDbContext _db;

    public ThongTinModel(UngDungDbContext db)
    {
        _db = db;
    }

    public NguoiDung? NguoiDungHienTai { get; set; }
    public List<DatTour> LichSuDatTour { get; set; } = new();

    // Chia lich su dat tour thanh 2 nhom de hien thi o tab My Bookings
    public List<DatTour> TourSapDi { get; set; } = new();
    public List<DatTour> TourDaDiHoacHuy { get; set; } = new();

    // Tong gia tri dat tour (demo, chua tach dat coc / con thieu)
    public decimal TongGiaTriDatTour { get; set; }

    public async Task OnGetAsync()
    {
        var tenDangNhap = User?.Identity?.Name;
        if (string.IsNullOrWhiteSpace(tenDangNhap))
        {
            NguoiDungHienTai = null;
            return;
        }

        NguoiDungHienTai = await _db.NguoiDungs
            .Include(n => n.VaiTro)
            .FirstOrDefaultAsync(n => n.TenDangNhap == tenDangNhap);

        if (NguoiDungHienTai != null)
        {
            LichSuDatTour = await _db.DatTours
                .Include(d => d.TourDuLich)
                .Where(d => d.NguoiDungId == NguoiDungHienTai.Id)
                .OrderByDescending(d => d.NgayDat)
                .ToListAsync();

            var today = DateTime.Today;
            TourSapDi = LichSuDatTour
                .Where(d => d.NgayKhoiHanh.Date >= today && !d.DaHuy)
                .OrderBy(d => d.NgayKhoiHanh)
                .ToList();

            TourDaDiHoacHuy = LichSuDatTour
                .Where(d => d.NgayKhoiHanh.Date < today || d.DaHuy)
                .OrderByDescending(d => d.NgayKhoiHanh)
                .ToList();

            // Tong tien chi tinh cac dat tour chua bi huy
            TongGiaTriDatTour = LichSuDatTour
                .Where(d => !d.DaHuy)
                .Sum(d => d.TongTien);
        }
    }

    public async Task<IActionResult> OnPostHuyAsync(int id)
    {
        var tenDangNhap = User?.Identity?.Name;
        if (string.IsNullOrWhiteSpace(tenDangNhap))
        {
            return RedirectToPage("/DangNhap");
        }

        var nguoiDung = await _db.NguoiDungs
            .FirstOrDefaultAsync(n => n.TenDangNhap == tenDangNhap);

        if (nguoiDung == null)
        {
            return RedirectToPage("/DangNhap");
        }

        var datTour = await _db.DatTours
            .Include(d => d.TourDuLich)
            .FirstOrDefaultAsync(d => d.Id == id && d.NguoiDungId == nguoiDung.Id);

        if (datTour == null)
        {
            TempData["ThongBaoTaiKhoan"] = "Không tìm thấy thông tin đặt tour cần hủy.";
            await OnGetAsync();
            return Page();
        }

        // Chi cho phep huy truoc ngay khoi hanh va chua huy
        if (datTour.DaHuy || datTour.NgayKhoiHanh.Date <= DateTime.Today)
        {
            TempData["ThongBaoTaiKhoan"] = "Không thể hủy tour đã khởi hành hoặc đã được đánh dấu hủy.";
            await OnGetAsync();
            return Page();
        }

        datTour.DaHuy = true;
        await _db.SaveChangesAsync();

        TempData["ThongBaoTaiKhoan"] = $"Đã hủy tour {(datTour.TourDuLich?.TenTour ?? ("#" + datTour.TourDuLichId))} thành công.";

        await OnGetAsync();
        return Page();
    }
}
