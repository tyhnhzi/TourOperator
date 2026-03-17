using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.Tour;

public class ChiTietModel : PageModel
{
    private readonly UngDungDbContext _db;

    public ChiTietModel(UngDungDbContext db)
    {
        _db = db;
    }

    [FromRoute]
    public int Id { get; set; }

    public TourDuLich? Tour { get; set; }

    public bool DaDangNhap => User?.Identity?.IsAuthenticated ?? false;

    public string? ThongBaoDatTour { get; set; }

    public class DatTourInput
    {
        [Required(ErrorMessage = "Vui lòng chọn ngày khởi hành.")]
        [DataType(DataType.Date)]
        public DateTime? NgayKhoiHanh { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số người lớn phải lớn hơn 0.")]
        public int SoNguoiLon { get; set; } = 1;

        [Range(0, int.MaxValue, ErrorMessage = "Số trẻ em không hợp lệ.")]
        public int SoTreEm { get; set; } = 0;

        [MaxLength(500)]
        public string? GhiChu { get; set; }
    }

    [BindProperty]
    public DatTourInput Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Tour = await _db.TourDuLichs.FirstOrDefaultAsync(t => t.Id == id);
        if (Tour == null)
        {
            return Page();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDatTourAsync(int id)
    {
        Tour = await _db.TourDuLichs.FirstOrDefaultAsync(t => t.Id == id);
        if (Tour == null)
        {
            ModelState.AddModelError(string.Empty, "Không tìm thấy tour để đặt.");
            return Page();
        }

        if (!(User?.Identity?.IsAuthenticated ?? false))
        {
            // Chuyển hướng sang trang đăng nhập với returnUrl quay lại trang chi tiết tour
            var returnUrl = Url.Page("/Tour/ChiTiet", new { id });
            return RedirectToPage("/DangNhap", new { returnUrl });
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var tenDangNhap = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name;
        var nguoiDung = await _db.NguoiDungs.FirstOrDefaultAsync(n => n.TenDangNhap == tenDangNhap);
        if (nguoiDung == null)
        {
            ModelState.AddModelError(string.Empty, "Không tìm thấy thông tin tài khoản để đặt tour.");
            return Page();
        }

        var tongSoKhach = Input.SoNguoiLon + Input.SoTreEm;
        var tongTien = tongSoKhach * Tour.GiaTu;

        var datTour = new DatTour
        {
            NguoiDungId = nguoiDung.Id,
            TourDuLichId = Tour.Id,
            NgayKhoiHanh = Input.NgayKhoiHanh!.Value,
            SoNguoiLon = Input.SoNguoiLon,
            SoTreEm = Input.SoTreEm,
            NgayDat = DateTime.UtcNow,
            TongTien = tongTien,
            GhiChu = Input.GhiChu
        };

        _db.DatTours.Add(datTour);
        await _db.SaveChangesAsync();

        ThongBaoDatTour = "Đặt tour thành công! Bạn có thể xem lại trong lịch sử đặt tour của tài khoản.";

        // Xóa dữ liệu input sau khi đặt thành công
        Input = new DatTourInput();

        return Page();
    }
}
