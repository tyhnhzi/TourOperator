using System.ComponentModel.DataAnnotations;

namespace TourOperatorWeb.Models;

public class DatXe
{
    public int Id { get; set; }

    [Required]
    public int NguoiDungId { get; set; }

    [Required]
    public int XeId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime NgayThue { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Số ngày thuê phải lớn hơn 0.")]
    public int SoNgay { get; set; } = 1;

    [StringLength(200)]
    public string? DiemDon { get; set; }

    [StringLength(200)]
    public string? DiemTra { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime NgayDat { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? GhiChu { get; set; }

    public NguoiDung? NguoiDung { get; set; }
    public Xe? Xe { get; set; }
}
