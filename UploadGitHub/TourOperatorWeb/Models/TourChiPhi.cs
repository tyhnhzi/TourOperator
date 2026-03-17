namespace TourOperatorWeb.Models;

public class TourChiPhi
{
    public int Id { get; set; }
    public int TourDuLichId { get; set; }
    public int DoiTacId { get; set; }
    public string MoTaDichVu { get; set; } = string.Empty;
    public decimal SoTien { get; set; }

    public TourDuLich? TourDuLich { get; set; }
    public DoiTac? DoiTac { get; set; }
}
