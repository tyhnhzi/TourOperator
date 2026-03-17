using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TourOperatorWeb.Pages.TinTuc;

public class IndexModel : PageModel
{
    public class TinTucItem
    {
        public string TieuDe { get; set; } = string.Empty;
        public string MoTaNgan { get; set; } = string.Empty;
        public DateTime NgayDang { get; set; }
        public int? TourId { get; set; }
        public string? LienKetTour { get; set; }
    }

    public IList<TinTucItem> DanhSachTin { get; set; } = new List<TinTucItem>();

    public void OnGet()
    {
        DanhSachTin = new List<TinTucItem>
        {
            new TinTucItem
            {
                TieuDe = "Gift voucher 500.000đ cho tour Đà Nẵng - Hội An",
                MoTaNgan = "Áp dụng cho khách đặt mới tour Đà Nẵng - Hội An 4N3Đ trong tháng này.",
                NgayDang = DateTime.Today.AddDays(-2),
                TourId = 1,
                LienKetTour = "/Tour/ChiTiet/1"
            },
            new TinTucItem
            {
                TieuDe = "Check-in Fansipan cùng VietTour, giảm ngay 10%",
                MoTaNgan = "Ưu đãi cho nhóm từ 4 khách trở lên khi đặt tour Hà Nội - Sapa - Fansipan 3N2Đ.",
                NgayDang = DateTime.Today.AddDays(-5),
                TourId = 2,
                LienKetTour = "/Tour/ChiTiet/2"
            },
            new TinTucItem
            {
                TieuDe = "Phú Quốc mùa biển đẹp nhất trong năm",
                MoTaNgan = "Gợi ý thời điểm lý tưởng để nghỉ dưỡng tại đảo ngọc Phú Quốc cùng gia đình.",
                NgayDang = DateTime.Today.AddDays(-10),
                TourId = 3,
                LienKetTour = "/Tour/ChiTiet/3"
            }
        };
    }
}
