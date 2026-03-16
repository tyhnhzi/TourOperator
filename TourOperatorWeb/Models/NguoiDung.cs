namespace TourOperatorWeb.Models;

public class NguoiDung
{
    public int Id { get; set; }
    public string TenDangNhap { get; set; } = string.Empty;
    public string MatKhau { get; set; } = string.Empty; // Demo: luu plain text (khong dung cho san pham that)

    // Thong tin ca nhan co ban (profile)
    public string? HoTen { get; set; }
    public string? SoDienThoai { get; set; }
    public string? Email { get; set; }

    // CCCD / Passport cho muc dich dat ve, bao hiem du lich
    public string? LoaiGiayTo { get; set; } // "CCCD" / "Passport" / ...
    public string? SoGiayTo { get; set; }

    // Cai dat nhan thong bao
    public bool NhanThongBaoEmail { get; set; } = true;
    public bool NhanThongBaoSms { get; set; } = false;

    public int VaiTroId { get; set; }
    public VaiTro? VaiTro { get; set; }
}
