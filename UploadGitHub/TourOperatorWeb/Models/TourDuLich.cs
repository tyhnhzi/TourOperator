using System.ComponentModel.DataAnnotations;

namespace TourOperatorWeb.Models;

public class TourDuLich
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Tên tour là bắt buộc.")]
    public string TenTour { get; set; } = string.Empty;

    [Required(ErrorMessage = "Điểm khởi hành là bắt buộc.")]
    public string DiaDiemKhoiHanh { get; set; } = string.Empty;

    [Required(ErrorMessage = "Điểm đến cuối là bắt buộc.")]
    public string DiaDiemDen { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Số ngày phải lớn hơn 0.")]
    public int SoNgay { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Giá từ phải lớn hơn hoặc bằng 0.")]
    public decimal GiaTu { get; set; }
    public string MoTaNgan { get; set; } = string.Empty;
    public string? AnhDaiDien { get; set; }
    public bool LaDeXuat { get; set; }

    public ICollection<DoanKhach> DoanKhachs { get; set; } = new List<DoanKhach>();
    public ICollection<TourChiPhi> ChiPhis { get; set; } = new List<TourChiPhi>();
    public ICollection<LichTrinhNgayTour> LichTrinhNgayTours { get; set; } = new List<LichTrinhNgayTour>();
    public ICollection<LichKhoiHanhTour> LichKhoiHanhs { get; set; } = new List<LichKhoiHanhTour>();
}
