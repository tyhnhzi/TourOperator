namespace TourOperatorWeb.Models;

public class LichTrinhNgayTour
{
    public int Id { get; set; }
    public int TourDuLichId { get; set; }
    public int NgayThu { get; set; }
    public string? DiemDen { get; set; }
    public string? HoatDong { get; set; }

    public TourDuLich? TourDuLich { get; set; }
}
