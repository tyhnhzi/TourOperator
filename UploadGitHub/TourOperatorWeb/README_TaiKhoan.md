# Tài khoản mẫu cho từng vai trò

Các tài khoản dưới đây đã được tạo sẵn trong cơ sở dữ liệu (seed dữ liệu) khi ứng dụng TourOperatorWeb khởi chạy lần đầu.

> Mật khẩu mặc định cho **tất cả** tài khoản: `123456`

## 1. Quản trị hệ thống (System Admin)
- Tên đăng nhập: **admin**
- Vai trò hệ thống: `QuanTriHeThong`
- Chức năng: Toàn quyền, tạo tài khoản, phân quyền, cấu hình danh mục gốc (địa điểm, đối tác, hướng dẫn viên,...).

## 2. Khách hàng
- Tên đăng nhập: **khach**
- Vai trò hệ thống: `KhachHang`
- Chức năng: Đăng nhập với tư cách khách hàng, xem thông tin tour.

---

## Ghi chú
- CSDL sử dụng: **SQLite** với file `TourOperatorWeb.db` (tự tạo khi chạy lần đầu).
- Dữ liệu seed nằm trong file:
  - `Data/UngDungDbContext.cs` (các vai trò và người dùng mẫu).
- Để đăng nhập:
  1. Vào trang **Đăng nhập** (`/DangNhap`).
  2. Nhập **tên đăng nhập** + **mật khẩu 123456**.
  3. Chọn **vai trò** tương ứng với tài khoản.
