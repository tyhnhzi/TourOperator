using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Data;

namespace TourOperatorWeb.Pages.Admin;

[Authorize(Roles = "QuanTriHeThong")]
public class QuanLyDoiTacModel : PageModel
{
    private readonly UngDungDbContext _nganHangDuLieu;

    public QuanLyDoiTacModel(UngDungDbContext nganHangDuLieu)
    {
        _nganHangDuLieu = nganHangDuLieu;
    }

    public List<DoiTacCongNoViewModel> DanhSachDoiTac { get; set; } = new();

    public class DoiTacCongNoViewModel
    {
        public string TenDoiTac { get; set; } = string.Empty;
        public string LoaiDoiTac { get; set; } = string.Empty;
        public decimal TongCongNo { get; set; }
    }

    public async Task OnGetAsync()
    {
        DanhSachDoiTac = await _nganHangDuLieu.DoiTacs
            .Include(d => d.ChiPhis)
            .Select(d => new DoiTacCongNoViewModel
            {
                TenDoiTac = d.TenDoiTac,
                LoaiDoiTac = d.LoaiDoiTac,
                TongCongNo = d.ChiPhis.Sum(c => c.SoTien)
            })
            .OrderByDescending(x => x.TongCongNo)
            .ToListAsync();
    }
}
