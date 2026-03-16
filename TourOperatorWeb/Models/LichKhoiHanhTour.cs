namespace TourOperatorWeb.Models;

public class LichKhoiHanhTour
{
    public int Id { get; set; }
    public int TourDuLichId { get; set; }
    public DateTime ThoiGianKhoiHanh { get; set; }
    public string PhuongTien { get; set; } = string.Empty;

    public TourDuLich? TourDuLich { get; set; }
}
