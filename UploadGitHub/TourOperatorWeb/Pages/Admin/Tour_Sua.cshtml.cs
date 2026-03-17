using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class Tour_SuaModel : PageModel
{
    private readonly UngDungDbContext _nganHangDuLieu;

    private readonly IWebHostEnvironment _moiTruongWeb;

    public Tour_SuaModel(UngDungDbContext nganHangDuLieu, IWebHostEnvironment moiTruongWeb)
    {
        _nganHangDuLieu = nganHangDuLieu;
        _moiTruongWeb = moiTruongWeb;
    }

    [BindProperty]
    public TourDuLich Tour { get; set; } = new();

    [BindProperty]
    public List<LichTrinhNgayInput> LichTrinhs { get; set; } = new();

    [BindProperty]
    public List<LichKhoiHanhInput> LichKhoiHanhs { get; set; } = new();

    [BindProperty]
    public IFormFile? AnhDaiDienFile { get; set; }

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

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var tour = await _nganHangDuLieu.TourDuLichs.FirstOrDefaultAsync(t => t.Id == id);
        if (tour == null)
        {
            return RedirectToPage("/Admin/QuanLyTour");
        }

        Tour = tour;

        // Tải lịch trình theo ngày
        LichTrinhs = await _nganHangDuLieu.LichTrinhNgayTours
            .Where(x => x.TourDuLichId == id)
            .OrderBy(x => x.NgayThu)
            .Select(x => new LichTrinhNgayInput
            {
                NgayThu = x.NgayThu,
                DiemDen = x.DiemDen,
                HoatDong = x.HoatDong
            })
            .ToListAsync();

        // Tải lịch khởi hành
        LichKhoiHanhs = await _nganHangDuLieu.LichKhoiHanhs
            .Where(x => x.TourDuLichId == id)
            .OrderBy(x => x.ThoiGianKhoiHanh)
            .Select(x => new LichKhoiHanhInput
            {
                ThoiGianKhoiHanh = x.ThoiGianKhoiHanh,
                PhuongTien = x.PhuongTien
            })
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Kiểm tra bắt buộc: ít nhất một lịch khởi hành đầy đủ
        var coLichKhoiHanhHopLe = false;
        if (LichKhoiHanhs != null)
        {
            foreach (var item in LichKhoiHanhs)
            {
                var coThoiGian = item.ThoiGianKhoiHanh.HasValue;
                var coPhuongTien = !string.IsNullOrWhiteSpace(item.PhuongTien);

                if (!coThoiGian && !coPhuongTien)
                {
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

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var tourDb = await _nganHangDuLieu.TourDuLichs.FirstOrDefaultAsync(t => t.Id == Tour.Id);
        if (tourDb == null)
        {
            return RedirectToPage("/Admin/QuanLyTour");
        }

        // Nếu upload ảnh mới thì lưu lại và cập nhật đường dẫn
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

            Tour.AnhDaiDien = $"/uploads/tours/{tenTep}";
        }

        tourDb.TenTour = Tour.TenTour;
        tourDb.DiaDiemKhoiHanh = Tour.DiaDiemKhoiHanh;
        tourDb.DiaDiemDen = Tour.DiaDiemDen;
        tourDb.SoNgay = Tour.SoNgay;
        tourDb.GiaTu = Tour.GiaTu;
        tourDb.MoTaNgan = Tour.MoTaNgan;
        tourDb.AnhDaiDien = Tour.AnhDaiDien;
        tourDb.LaDeXuat = Tour.LaDeXuat;

        // Cập nhật lại lịch trình theo ngày: xóa cũ, thêm mới
        var lichTrinhCu = _nganHangDuLieu.LichTrinhNgayTours.Where(x => x.TourDuLichId == tourDb.Id);
        _nganHangDuLieu.LichTrinhNgayTours.RemoveRange(lichTrinhCu);

        if (LichTrinhs != null && LichTrinhs.Count > 0)
        {
            foreach (var item in LichTrinhs)
            {
                if (item.NgayThu <= 0)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(item.DiemDen) && string.IsNullOrWhiteSpace(item.HoatDong))
                {
                    continue;
                }

                var lich = new LichTrinhNgayTour
                {
                    TourDuLichId = tourDb.Id,
                    NgayThu = item.NgayThu,
                    DiemDen = item.DiemDen,
                    HoatDong = item.HoatDong
                };

                _nganHangDuLieu.LichTrinhNgayTours.Add(lich);
            }
        }

        // Cập nhật lịch khởi hành: xóa cũ, thêm mới
        var lichKhoiHanhCu = _nganHangDuLieu.LichKhoiHanhs.Where(x => x.TourDuLichId == tourDb.Id);
        _nganHangDuLieu.LichKhoiHanhs.RemoveRange(lichKhoiHanhCu);

        if (LichKhoiHanhs != null && LichKhoiHanhs.Count > 0)
        {
            foreach (var item in LichKhoiHanhs)
            {
                if (!item.ThoiGianKhoiHanh.HasValue && string.IsNullOrWhiteSpace(item.PhuongTien))
                {
                    continue;
                }

                if (!item.ThoiGianKhoiHanh.HasValue)
                {
                    continue;
                }

                var lichKhoiHanh = new LichKhoiHanhTour
                {
                    TourDuLichId = tourDb.Id,
                    ThoiGianKhoiHanh = item.ThoiGianKhoiHanh.Value,
                    PhuongTien = item.PhuongTien
                };

                _nganHangDuLieu.LichKhoiHanhs.Add(lichKhoiHanh);
            }
        }

        await _nganHangDuLieu.SaveChangesAsync();

        // Khi mở trong iframe (modal), trả về script yêu cầu trang cha reload để thấy thay đổi
        return Content("<script>window.parent.location.reload();</script>", "text/html");
    }
}
