using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;
using KhachSanEntity = TourOperatorWeb.Models.KhachSan;

namespace TourOperatorWeb.Pages.KhachSanPages;

public class DatModel : PageModel
{
    private readonly UngDungDbContext _db;

    public DatModel(UngDungDbContext db)
    {
        _db = db;
    }

    public KhachSanEntity? KhachSan { get; set; }

    [BindProperty]
    public DatKhachSanInput Input { get; set; } = new();

    public string? ThongBao { get; set; }

    public class DatKhachSanInput
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime NgayNhanPhong { get; set; } = DateTime.Today.AddDays(7);

        [Required]
        [DataType(DataType.Date)]
        public DateTime NgayTraPhong { get; set; } = DateTime.Today.AddDays(9);

        [Range(1, int.MaxValue)]
        public int SoPhong { get; set; } = 1;

        [Range(1, int.MaxValue)]
        public int SoKhach { get; set; } = 1;

        [MaxLength(500)]
        public string? GhiChu { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        KhachSan = await _db.KhachSans.FirstOrDefaultAsync(k => k.Id == id);
        if (KhachSan == null)
        {
            return RedirectToPage("/KhachSan/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var tenDangNhap = User?.Identity?.Name;
        if (string.IsNullOrWhiteSpace(tenDangNhap))
        {
            return RedirectToPage("/DangNhap", new { returnUrl = Url.Page("/KhachSan/Dat", new { id }) });
        }

        KhachSan = await _db.KhachSans.FirstOrDefaultAsync(k => k.Id == id);
        if (KhachSan == null)
        {
            return RedirectToPage("/KhachSan/Index");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Input.NgayTraPhong <= Input.NgayNhanPhong)
        {
            ModelState.AddModelError(string.Empty, "Ngày trả phòng phải sau ngày nhận phòng.");
            return Page();
        }

        var nguoiDung = await _db.NguoiDungs.FirstOrDefaultAsync(n => n.TenDangNhap == tenDangNhap);
        if (nguoiDung == null)
        {
            return RedirectToPage("/DangNhap");
        }

        var dat = new DatKhachSan
        {
            NguoiDungId = nguoiDung.Id,
            KhachSanId = KhachSan.Id,
            NgayNhanPhong = Input.NgayNhanPhong,
            NgayTraPhong = Input.NgayTraPhong,
            SoPhong = Input.SoPhong,
            SoKhach = Input.SoKhach,
            GhiChu = Input.GhiChu,
            NgayDat = DateTime.UtcNow
        };

        _db.DatKhachSans.Add(dat);
        await _db.SaveChangesAsync();

        ThongBao = "Đã ghi nhận yêu cầu đặt phòng.";
        Input = new DatKhachSanInput();
        return Page();
    }
}
