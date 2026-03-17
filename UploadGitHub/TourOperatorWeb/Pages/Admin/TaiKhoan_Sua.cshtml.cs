using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class TaiKhoan_SuaModel : PageModel
{
    private readonly UngDungDbContext _db;

    public TaiKhoan_SuaModel(UngDungDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public List<SelectListItem> DanhSachVaiTro { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        await NapDanhSachVaiTroAsync();

        var nguoiDung = await _db.NguoiDungs.FirstOrDefaultAsync(n => n.Id == id);
        if (nguoiDung == null)
        {
            return RedirectToPage("/Admin/QuanLyTaiKhoan");
        }

        Input = new InputModel
        {
            Id = nguoiDung.Id,
            TenDangNhap = nguoiDung.TenDangNhap,
            VaiTroId = nguoiDung.VaiTroId
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await NapDanhSachVaiTroAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var nguoiDung = await _db.NguoiDungs.FirstOrDefaultAsync(n => n.Id == Input.Id);
        if (nguoiDung == null)
        {
            return RedirectToPage("/Admin/QuanLyTaiKhoan");
        }

        // Kiem tra trung ten dang nhap (ngoai tru chinh no)
        var trungTen = await _db.NguoiDungs
            .AnyAsync(n => n.Id != Input.Id && n.TenDangNhap == Input.TenDangNhap);
        if (trungTen)
        {
            ModelState.AddModelError("Input.TenDangNhap", "Tên đăng nhập đã được sử dụng bởi tài khoản khác.");
            return Page();
        }

        nguoiDung.TenDangNhap = Input.TenDangNhap.Trim();
        nguoiDung.VaiTroId = Input.VaiTroId;

        if (!string.IsNullOrWhiteSpace(Input.MatKhauMoi))
        {
            nguoiDung.MatKhau = Input.MatKhauMoi.Trim();
        }

        await _db.SaveChangesAsync();

        // Khi mở trong iframe (modal), trả về script yêu cầu trang cha reload để cập nhật danh sách
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
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 50 ký tự.")]
        public string TenDangNhap { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu mới phải từ 6 đến 50 ký tự.")]
        [DataType(DataType.Password)]
        public string? MatKhauMoi { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        public int VaiTroId { get; set; }
    }
}
