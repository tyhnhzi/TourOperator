namespace TourOperatorWeb.Models;

public class VaiTro
{
    public int Id { get; set; }
    public string TenVaiTro { get; set; } = string.Empty; // VD: QuanTriHeThong, NhanVienDieuHanhTour

    public ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
