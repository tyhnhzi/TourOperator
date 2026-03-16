using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;

namespace TourOperatorWeb.Pages;

public class DangNhapModel : PageModel
{
    private readonly UngDungDbContext _nganHangDuLieu;

    public DangNhapModel(UngDungDbContext nganHangDuLieu)
    {
        _nganHangDuLieu = nganHangDuLieu;
    }

    [BindProperty]
    public string TenDangNhap { get; set; } = string.Empty;

    [BindProperty]
    public string MatKhau { get; set; } = string.Empty;

    [BindProperty]
    public int VaiTroId { get; set; }

    public List<SelectListItem> DanhSachVaiTro { get; set; } = new();

    public string ThongBaoLoi { get; set; } = string.Empty;

    public async Task OnGetAsync(string? returnUrl = null)
    {
        await NapDanhSachVaiTroAsync();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        await NapDanhSachVaiTroAsync();

        if (string.IsNullOrWhiteSpace(TenDangNhap) || string.IsNullOrWhiteSpace(MatKhau) || VaiTroId == 0)
        {
            ThongBaoLoi = "Vui lòng nhập đầy đủ thông tin và chọn vai trò.";
            return Page();
        }

        var nguoiDung = await _nganHangDuLieu.NguoiDungs
            .Include(n => n.VaiTro)
            .FirstOrDefaultAsync(n => n.TenDangNhap == TenDangNhap && n.MatKhau == MatKhau && n.VaiTroId == VaiTroId);

        if (nguoiDung == null)
        {
            ThongBaoLoi = "Thông tin đăng nhập không chính xác.";
            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, nguoiDung.Id.ToString()),
            new Claim(ClaimTypes.Name, nguoiDung.TenDangNhap),
            new Claim(ClaimTypes.Role, nguoiDung.VaiTro?.TenVaiTro ?? string.Empty)
        };

        var danhTinh = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var doiTuongDangNhap = new ClaimsPrincipal(danhTinh);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, doiTuongDangNhap);

        // Neu la quan tri he thong thi chuyen thang den giao dien admin
        if (nguoiDung.VaiTro?.TenVaiTro == "QuanTriHeThong")
        {
            return RedirectToPage("/Admin/TrangChuAdmin");
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        // Mac dinh chuyen ve trang chu khach hang
        return RedirectToPage("/Index");
    }

    private async Task NapDanhSachVaiTroAsync()
    {
        DanhSachVaiTro = await _nganHangDuLieu.VaiTros
            .OrderBy(v => v.Id)
            .Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.TenVaiTro
            })
            .ToListAsync();
    }
}
