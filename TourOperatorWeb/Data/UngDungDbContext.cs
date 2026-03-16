using Microsoft.EntityFrameworkCore;
using TourOperatorWeb.Models;

namespace TourOperatorWeb.Data;

public class UngDungDbContext : DbContext
{
    public UngDungDbContext(DbContextOptions<UngDungDbContext> options) : base(options)
    {
    }

    public DbSet<NguoiDung> NguoiDungs => Set<NguoiDung>();
    public DbSet<VaiTro> VaiTros => Set<VaiTro>();
    public DbSet<TourDuLich> TourDuLichs => Set<TourDuLich>();
    public DbSet<DoiTac> DoiTacs => Set<DoiTac>();
    public DbSet<DoanKhach> DoanKhachs => Set<DoanKhach>();
    public DbSet<TourChiPhi> TourChiPhis => Set<TourChiPhi>();
    public DbSet<LichTrinhNgayTour> LichTrinhNgayTours => Set<LichTrinhNgayTour>();
    public DbSet<LichKhoiHanhTour> LichKhoiHanhs => Set<LichKhoiHanhTour>();
    public DbSet<KhachSan> KhachSans => Set<KhachSan>();
    public DbSet<Xe> Xes => Set<Xe>();
    public DbSet<DatTour> DatTours => Set<DatTour>();
    public DbSet<DatKhachSan> DatKhachSans => Set<DatKhachSan>();
    public DbSet<DatXe> DatXes => Set<DatXe>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed du lieu vai tro (chi giu lai QuanTriHeThong va KhachHang)
        modelBuilder.Entity<VaiTro>().HasData(
            new VaiTro { Id = 1, TenVaiTro = "QuanTriHeThong" },
            new VaiTro { Id = 2, TenVaiTro = "KhachHang" }
        );

        // Seed demo nguoi dung (chi giu lai admin va khach)
        modelBuilder.Entity<NguoiDung>().HasData(
            new NguoiDung
            {
                Id = 1,
                TenDangNhap = "admin",
                MatKhau = "123456",
                VaiTroId = 1,
                HoTen = "Quản trị hệ thống",
                SoDienThoai = "0900 000 001",
                Email = "admin@viettour.demo",
                LoaiGiayTo = "CCCD",
                SoGiayTo = "012345678901",
                NhanThongBaoEmail = true,
                NhanThongBaoSms = false
            },
            new NguoiDung
            {
                Id = 2,
                TenDangNhap = "khach",
                MatKhau = "123456",
                VaiTroId = 2,
                HoTen = "Khách hàng demo",
                SoDienThoai = "0900 000 002",
                Email = "khach@viettour.demo",
                LoaiGiayTo = "Passport",
                SoGiayTo = "B1234567",
                NhanThongBaoEmail = true,
                NhanThongBaoSms = true
            }
        );

        // Seed mot so tour de xuat
        modelBuilder.Entity<TourDuLich>().HasData(
            new TourDuLich
            {
                Id = 1,
                TenTour = "Đà Nẵng - Hội An 4N3Đ",
                DiaDiemKhoiHanh = "TP. Hồ Chí Minh",
                DiaDiemDen = "Đà Nẵng, Hội An",
                SoNgay = 4,
                GiaTu = 5990000,
                MoTaNgan = "Khám phá biển Mỹ Khê, Bà Nà Hills và phố cổ Hội An lung linh đèn lồng.",
                AnhDaiDien = "/images/tour-danang.jpg",
                LaDeXuat = true
            },
            new TourDuLich
            {
                Id = 2,
                TenTour = "Hà Nội - Sapa - Fansipan 3N2Đ",
                DiaDiemKhoiHanh = "Hà Nội",
                DiaDiemDen = "Sapa, Fansipan",
                SoNgay = 3,
                GiaTu = 4890000,
                MoTaNgan = "Trải nghiệm khí hậu mát lạnh, ruộng bậc thang và chinh phục nóc nhà Đông Dương.",
                AnhDaiDien = "/images/tour-sapa.jpg",
                LaDeXuat = true
            },
            new TourDuLich
            {
                Id = 3,
                TenTour = "Phú Quốc nghỉ dưỡng 3N2Đ",
                DiaDiemKhoiHanh = "TP. Hồ Chí Minh",
                DiaDiemDen = "Phú Quốc",
                SoNgay = 3,
                GiaTu = 5290000,
                MoTaNgan = "Nghỉ dưỡng tại đảo ngọc Phú Quốc, tắm biển và thưởng thức hải sản tươi sống.",
                AnhDaiDien = "/images/tour-phuquoc.jpg",
                LaDeXuat = true
            }
        );

        // Seed doi tac (nha hang, khach san)
        modelBuilder.Entity<DoiTac>().HasData(
            new DoiTac { Id = 1, TenDoiTac = "Nhà hàng Biển Xanh", LoaiDoiTac = "NhaHang", GhiChu = "Ăn trưa, ăn tối tour biển" },
            new DoiTac { Id = 2, TenDoiTac = "Khách sạn Biển Ngọc", LoaiDoiTac = "KhachSan", GhiChu = "Khách sạn 3 sao tại Đà Nẵng" },
            new DoiTac { Id = 3, TenDoiTac = "Nhà hàng Núi Rừng", LoaiDoiTac = "NhaHang", GhiChu = "Ẩm thực vùng cao cho tour Sapa" },
            new DoiTac { Id = 4, TenDoiTac = "Khách sạn Mường Hoa", LoaiDoiTac = "KhachSan", GhiChu = "Khách sạn 3 sao tại Sapa" },
            new DoiTac { Id = 5, TenDoiTac = "Nhà hàng Hải Sản Phú Quốc", LoaiDoiTac = "NhaHang", GhiChu = "Hải sản cho tour Phú Quốc" },
            new DoiTac { Id = 6, TenDoiTac = "Resort Biển Đảo", LoaiDoiTac = "KhachSan", GhiChu = "Resort 4 sao tại Phú Quốc" }
        );

        // Seed chi phi tour (tra doi tac)
        modelBuilder.Entity<TourChiPhi>().HasData(
            // Tour 1: Đà Nẵng - Hội An
            new TourChiPhi { Id = 1, TourDuLichId = 1, DoiTacId = 1, MoTaDichVu = "Ăn chính tại Đà Nẵng và Hội An", SoTien = 1500000 },
            new TourChiPhi { Id = 2, TourDuLichId = 1, DoiTacId = 2, MoTaDichVu = "Lưu trú 3 đêm khách sạn Biển Ngọc", SoTien = 2200000 },

            // Tour 2: Hà Nội - Sapa - Fansipan
            new TourChiPhi { Id = 3, TourDuLichId = 2, DoiTacId = 3, MoTaDichVu = "Ăn chính tại Sapa", SoTien = 1200000 },
            new TourChiPhi { Id = 4, TourDuLichId = 2, DoiTacId = 4, MoTaDichVu = "Lưu trú 2 đêm khách sạn Mường Hoa", SoTien = 1800000 },

            // Tour 3: Phú Quốc
            new TourChiPhi { Id = 5, TourDuLichId = 3, DoiTacId = 5, MoTaDichVu = "Ăn hải sản Phú Quốc", SoTien = 1400000 },
            new TourChiPhi { Id = 6, TourDuLichId = 3, DoiTacId = 6, MoTaDichVu = "Lưu trú 2 đêm resort Biển Đảo", SoTien = 2600000 }
        );

        // Seed doan khach cho tung tour (doanh thu khach tra)
        modelBuilder.Entity<DoanKhach>().HasData(
            // Tour 1
            new DoanKhach { Id = 1, TourDuLichId = 1, TenDoan = "Đoàn công ty ABC", SoKhach = 20, DonGiaMotKhach = 7990000 },
            new DoanKhach { Id = 2, TourDuLichId = 1, TenDoan = "Gia đình mở rộng", SoKhach = 12, DonGiaMotKhach = 7490000 },

            // Tour 2
            new DoanKhach { Id = 3, TourDuLichId = 2, TenDoan = "Đoàn khách lẻ ghép tour", SoKhach = 15, DonGiaMotKhach = 6890000 },

            // Tour 3
            new DoanKhach { Id = 4, TourDuLichId = 3, TenDoan = "Đoàn công ty XYZ", SoKhach = 25, DonGiaMotKhach = 8290000 }
        );

        // Seed mot so khach san
        modelBuilder.Entity<KhachSan>().HasData(
            new KhachSan { Id = 1, TenKhachSan = "Khách sạn Biển Ngọc", DiaChi = "Đà Nẵng", SoDienThoai = "0236 111 222" },
            new KhachSan { Id = 2, TenKhachSan = "Khách sạn Mường Hoa", DiaChi = "Sapa", SoDienThoai = "0214 333 444" }
        );

        // Seed mot so xe
        modelBuilder.Entity<Xe>().HasData(
            new Xe { Id = 1, BienSo = "51A-123.45", LoaiXe = "Xe 29 chỗ", SoChoNgoi = 29, TenTaiXe = "Nguyễn Văn A" },
            new Xe { Id = 2, BienSo = "30B-678.90", LoaiXe = "Xe 16 chỗ", SoChoNgoi = 16, TenTaiXe = "Trần Thị B" }
        );
    }
}
