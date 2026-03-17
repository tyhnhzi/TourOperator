using System.ComponentModel.DataAnnotations;

namespace TourOperatorWeb.Models;

public class KhachSan
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên khách sạn.")]
    [StringLength(100, ErrorMessage = "Tên khách sạn tối đa 100 ký tự.")]
    public string TenKhachSan { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Địa chỉ tối đa 200 ký tự.")]
    public string? DiaChi { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
    [StringLength(20)]
    public string? SoDienThoai { get; set; }
}
