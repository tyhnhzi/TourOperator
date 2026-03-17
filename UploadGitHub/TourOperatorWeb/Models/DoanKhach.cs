namespace TourOperatorWeb.Models;

public class DoanKhach
{
    public int Id { get; set; }
    public int TourDuLichId { get; set; }
    public string TenDoan { get; set; } = string.Empty;
    public int SoKhach { get; set; }
    public decimal DonGiaMotKhach { get; set; }

    public TourDuLich? TourDuLich { get; set; }

    // Tổng thu của đoàn = Số khách * Đơn giá/khách
    public decimal TongThu => SoKhach * DonGiaMotKhach;
}
