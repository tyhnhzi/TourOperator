using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class TaiKhoan_ThemModel : PageModel
{
    private readonly UngDungDbContext _db;

    public TaiKhoan_ThemModel(UngDungDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public List<SelectListItem> DanhSachVaiTro { get; set; } = new();

    public async Task OnGetAsync()
    {
        await NapDanhSachVaiTroAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await NapDanhSachVaiTroAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Kiem tra trung ten dang nhap
        var tonTai = await _db.NguoiDungs.AnyAsync(n => n.TenDangNhap == Input.TenDangNhap);
        if (tonTai)
        {
            ModelState.AddModelError("Input.TenDangNhap", "Tên đăng nhập đã tồn tại.");
            return Page();
        }

        var nguoiDung = new NguoiDung
        {
            TenDangNhap = Input.TenDangNhap.Trim(),
            MatKhau = Input.MatKhau.Trim(),
            VaiTroId = Input.VaiTroId
        };

        _db.NguoiDungs.Add(nguoiDung);
        await _db.SaveChangesAsync();

        // Khi mở trong iframe (modal), trả về script yêu cầu trang cha reload để thấy tài khoản mới
        return Content("<script>window.parent.location.reload();</script>", "text/html");
    }

    private async Task NapDanhSachVaiTroAsync()
    {
        DanhSachVaiTro = await _db.VaiTros
            .OrderBy(v => v.Id)
            .Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.TenVaiTro
            })
            .ToListAsync();
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 50 ký tự.")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 50 ký tự.")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        public int VaiTroId { get; set; }
    }
}
