using System.ComponentModel.DataAnnotations;

namespace TourOperatorWeb.Models;

public class DatTour
{
    public int Id { get; set; }

    [Required]
    public int NguoiDungId { get; set; }

    [Required]
    public int TourDuLichId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime NgayKhoiHanh { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Số người lớn phải lớn hơn 0.")]
    public int SoNguoiLon { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số trẻ em không hợp lệ.")]
    public int SoTreEm { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime NgayDat { get; set; } = DateTime.UtcNow;

    [Range(0, double.MaxValue)]
    public decimal TongTien { get; set; }

    [MaxLength(500)]
    public string? GhiChu { get; set; }

    // Danh dau dat tour da bi huy (demo)
    public bool DaHuy { get; set; }

    public NguoiDung? NguoiDung { get; set; }
    public TourDuLich? TourDuLich { get; set; }
}
