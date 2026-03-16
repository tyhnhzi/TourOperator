using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.ThueXe;

public class DatModel : PageModel
{
    private readonly UngDungDbContext _db;

    public DatModel(UngDungDbContext db)
    {
        _db = db;
    }

    public Xe? Xe { get; set; }

    [BindProperty]
    public DatXeInput Input { get; set; } = new();

    public string? ThongBao { get; set; }

    public class DatXeInput
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime NgayThue { get; set; } = DateTime.Today.AddDays(7);

        [Range(1, int.MaxValue)]
        public int SoNgay { get; set; } = 1;

        [StringLength(200)]
        public string? DiemDon { get; set; }

        [StringLength(200)]
        public string? DiemTra { get; set; }

        [MaxLength(500)]
        public string? GhiChu { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Xe = await _db.Xes.FirstOrDefaultAsync(x => x.Id == id);
        if (Xe == null)
        {
            return RedirectToPage("/ThueXe/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var tenDangNhap = User?.Identity?.Name;
        if (string.IsNullOrWhiteSpace(tenDangNhap))
        {
            return RedirectToPage("/DangNhap", new { returnUrl = Url.Page("/ThueXe/Dat", new { id }) });
        }

        Xe = await _db.Xes.FirstOrDefaultAsync(x => x.Id == id);
        if (Xe == null)
        {
            return RedirectToPage("/ThueXe/Index");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var nguoiDung = await _db.NguoiDungs.FirstOrDefaultAsync(n => n.TenDangNhap == tenDangNhap);
        if (nguoiDung == null)
        {
            return RedirectToPage("/DangNhap");
        }

        var dat = new DatXe
        {
            NguoiDungId = nguoiDung.Id,
            XeId = Xe.Id,
            NgayThue = Input.NgayThue,
            SoNgay = Input.SoNgay,
            DiemDon = Input.DiemDon,
            DiemTra = Input.DiemTra,
            GhiChu = Input.GhiChu,
            NgayDat = DateTime.UtcNow
        };

        _db.DatXes.Add(dat);
        await _db.SaveChangesAsync();

        ThongBao = "Đã ghi nhận yêu cầu thuê xe.";
        Input = new DatXeInput();
        return Page();
    }
}
