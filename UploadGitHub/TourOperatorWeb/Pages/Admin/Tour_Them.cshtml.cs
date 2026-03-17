using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class Tour_ThemModel : PageModel
{
    private readonly UngDungDbContext _nganHangDuLieu;

    private readonly IWebHostEnvironment _moiTruongWeb;

    public Tour_ThemModel(UngDungDbContext nganHangDuLieu, IWebHostEnvironment moiTruongWeb)
    {
        _nganHangDuLieu = nganHangDuLieu;
        _moiTruongWeb = moiTruongWeb;
    }

    [BindProperty]
    public TourDuLich Tour { get; set; } = new();

    [BindProperty]
    public List<LichTrinhNgayInput> LichTrinhs { get; set; } = new();

    [BindProperty]
    public IFormFile? AnhDaiDienFile { get; set; }

    [BindProperty]
    public List<LichKhoiHanhInput> LichKhoiHanhs { get; set; } = new();

    public class LichTrinhNgayInput
    {
        public int NgayThu { get; set; }
        public string? DiemDen { get; set; }
        public string? HoatDong { get; set; }
    }

    public class LichKhoiHanhInput
    {
        public DateTime? ThoiGianKhoiHanh { get; set; }
        public string PhuongTien { get; set; } = string.Empty;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Kiểm tra bắt buộc: ảnh đại diện và ít nhất một lịch khởi hành đầy đủ
        if (AnhDaiDienFile == null || AnhDaiDienFile.Length == 0)
        {
            ModelState.AddModelError("AnhDaiDienFile", "Vui lòng chọn ảnh đại diện cho tour.");
        }

        // Kiểm tra: điểm đến cho từng ngày là bắt buộc
        var thieuDiemDen = false;
        if (LichTrinhs != null && LichTrinhs.Count > 0)
        {
            foreach (var item in LichTrinhs)
            {
                if (item.NgayThu <= 0)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(item.DiemDen))
                {
                    thieuDiemDen = true;
                    break;
                }
            }
        }

        if (thieuDiemDen)
        {
            ModelState.AddModelError("LichTrinhs", "Vui lòng nhập điểm đến cho từng ngày (không được để trống).");
        }

        var coLichKhoiHanhHopLe = false;
        if (LichKhoiHanhs != null)
        {
            foreach (var item in LichKhoiHanhs)
            {
                var coThoiGian = item.ThoiGianKhoiHanh.HasValue;
                var coPhuongTien = !string.IsNullOrWhiteSpace(item.PhuongTien);

                if (!coThoiGian && !coPhuongTien)
                {
                    // dòng trống hoàn toàn, bỏ qua
                    continue;
                }

                if (coThoiGian && coPhuongTien)
                {
                    coLichKhoiHanhHopLe = true;
                }
                else
                {
                    ModelState.AddModelError("LichKhoiHanhs", "Mỗi lịch khởi hành cần đủ cả ngày giờ và phương tiện.");
                }
            }
        }

        if (!coLichKhoiHanhHopLe)
        {
            ModelState.AddModelError("LichKhoiHanhs", "Vui lòng nhập ít nhất một lịch khởi hành (ngày giờ và phương tiện).");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Xử lý upload ảnh đại diện nếu có
        if (AnhDaiDienFile != null && AnhDaiDienFile.Length > 0)
        {
            var thuMucUpload = Path.Combine(_moiTruongWeb.WebRootPath, "uploads", "tours");
            Directory.CreateDirectory(thuMucUpload);

            var tenMoRong = Path.GetExtension(AnhDaiDienFile.FileName);
            var tenTep = $"{Guid.NewGuid()}{tenMoRong}";
            var duongDanTuyetDoi = Path.Combine(thuMucUpload, tenTep);

            using (var stream = System.IO.File.Create(duongDanTuyetDoi))
            {
                await AnhDaiDienFile.CopyToAsync(stream);
            }

            // Lưu đường dẫn tương đối để hiển thị trên web
            Tour.AnhDaiDien = $"/uploads/tours/{tenTep}";
        }

        _nganHangDuLieu.TourDuLichs.Add(Tour);
        await _nganHangDuLieu.SaveChangesAsync();

        // Lưu lịch trình từng ngày nếu người dùng đã nhập
        if (LichTrinhs != null && LichTrinhs.Count > 0)
        {
            foreach (var item in LichTrinhs)
            {
                if (item.NgayThu <= 0)
                {
                    continue;
                }

                // Bỏ qua dòng trống hoàn toàn
                if (string.IsNullOrWhiteSpace(item.DiemDen) && string.IsNullOrWhiteSpace(item.HoatDong))
                {
                    continue;
                }

                var lich = new LichTrinhNgayTour
                {
                    TourDuLichId = Tour.Id,
                    NgayThu = item.NgayThu,
                    DiemDen = item.DiemDen,
                    HoatDong = item.HoatDong
                };

                _nganHangDuLieu.LichTrinhNgayTours.Add(lich);
            }

            await _nganHangDuLieu.SaveChangesAsync();
        }

        // Lưu lịch khởi hành nếu có
        if (LichKhoiHanhs != null && LichKhoiHanhs.Count > 0)
        {
            foreach (var lichInput in LichKhoiHanhs)
            {
                if (!lichInput.ThoiGianKhoiHanh.HasValue && string.IsNullOrWhiteSpace(lichInput.PhuongTien))
                {
                    continue;
                }

                if (!lichInput.ThoiGianKhoiHanh.HasValue)
                {
                    continue;
                }

                var lichKhoiHanh = new LichKhoiHanhTour
                {
                    TourDuLichId = Tour.Id,
                    ThoiGianKhoiHanh = lichInput.ThoiGianKhoiHanh.Value,
                    PhuongTien = lichInput.PhuongTien
                };

                _nganHangDuLieu.LichKhoiHanhs.Add(lichKhoiHanh);
            }

            await _nganHangDuLieu.SaveChangesAsync();
        }

        // Khi mở trong iframe (modal), trả về script yêu cầu trang cha reload để thấy tour mới
        return Content("<script>window.parent.location.reload();</script>", "text/html");
    }
}
