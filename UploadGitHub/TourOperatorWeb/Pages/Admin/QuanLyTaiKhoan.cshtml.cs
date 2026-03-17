using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;

namespace TourOperatorWeb.Pages.Admin;

public class QuanLyTaiKhoanModel : PageModel
{
    private readonly UngDungDbContext _nganHangDuLieu;

    public QuanLyTaiKhoanModel(UngDungDbContext nganHangDuLieu)
    {
        _nganHangDuLieu = nganHangDuLieu;
    }

    public List<DongTaiKhoanViewModel> DanhSachNguoiDung { get; set; } = new();

    [Authorize(Roles = "QuanTriHeThong")]
    public async Task OnGetAsync()
    {
        DanhSachNguoiDung = await _nganHangDuLieu.NguoiDungs
            .Include(n => n.VaiTro)
            .OrderBy(n => n.TenDangNhap)
            .Select(n => new DongTaiKhoanViewModel
            {
                Id = n.Id,
                TenDangNhap = n.TenDangNhap,
                TenVaiTro = n.VaiTro != null ? n.VaiTro.TenVaiTro : string.Empty
            })
            .ToListAsync();
    }

    public class DongTaiKhoanViewModel
    {
        public int Id { get; set; }
        public string TenDangNhap { get; set; } = string.Empty;
        public string TenVaiTro { get; set; } = string.Empty;
    }
}
