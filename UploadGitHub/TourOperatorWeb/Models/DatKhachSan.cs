using System.ComponentModel.DataAnnotations;

namespace TourOperatorWeb.Models;

public class DatKhachSan
{
    public int Id { get; set; }

    [Required]
    public int NguoiDungId { get; set; }

    [Required]
    public int KhachSanId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime NgayNhanPhong { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime NgayTraPhong { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Số phòng phải lớn hơn 0.")]
    public int SoPhong { get; set; } = 1;

    [Range(1, int.MaxValue, ErrorMessage = "Số khách phải lớn hơn 0.")]
    public int SoKhach { get; set; } = 1;

    [DataType(DataType.DateTime)]
    public DateTime NgayDat { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? GhiChu { get; set; }

    public NguoiDung? NguoiDung { get; set; }
    public KhachSan? KhachSan { get; set; }
}
