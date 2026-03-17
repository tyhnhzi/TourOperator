using System.ComponentModel.DataAnnotations;

namespace TourOperatorWeb.Models;

public class Xe
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập biển số xe.")]
    [StringLength(20, ErrorMessage = "Biển số tối đa 20 ký tự.")]
    public string BienSo { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Loại xe tối đa 50 ký tự.")]
    public string? LoaiXe { get; set; }

    [Range(1, 60, ErrorMessage = "Số chỗ ngồi phải từ 1 đến 60.")]
    public int? SoChoNgoi { get; set; }

    [StringLength(100, ErrorMessage = "Tên tài xế tối đa 100 ký tự.")]
    public string? TenTaiXe { get; set; }
}
