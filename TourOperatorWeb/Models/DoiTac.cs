namespace TourOperatorWeb.Models;

public class DoiTac
{
    public int Id { get; set; }
    public string TenDoiTac { get; set; } = string.Empty;
    public string LoaiDoiTac { get; set; } = string.Empty; // Ví dụ: NhaHang, KhachSan
    public string? GhiChu { get; set; }

    public ICollection<TourChiPhi> ChiPhis { get; set; } = new List<TourChiPhi>();
}
