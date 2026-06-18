# 🚀 LOI_CMS - PHÁT TRIỂN API & FRONTEND (BUỔI 10)

Chào mừng bạn đến với cập nhật của **Buổi 10** trong dự án **LOI_CMS**. Trong buổi này, hệ thống tập trung cải tiến mạnh mẽ vào trải nghiệm người dùng (UX) ở giao diện Frontend, đồng thời bổ sung các tính năng bảo mật quan trọng (Authentication) và quản lý tài nguyên đa phương tiện ở Backend.

---

## 📌 Các Tính Năng Mới & Cập Nhật (Buổi 10)

### 1. Nâng Cấp Giao Diện & UX (Header)
* **Sticky Header:** Cấu hình thanh điều hướng luôn cố định ở trên cùng khi cuộn trang, giúp tối ưu không gian trải nghiệm và truy cập menu nhanh chóng.
* **Instant Search & Render:** Tích hợp bộ lọc tìm kiếm thông minh ngay trên Header. Kết quả tìm kiếm được xử lý và hiển thị (render) ngay lập tức khi người dùng nhập từ khóa mà không cần tải lại trang.

### 2. Tính Năng Xác Thực & Hệ Thống (Authentication & Mail)
* **Forgot Password:** Xây dựng luồng khôi phục tài khoản hoàn chỉnh cho người dùng khi quên mật khẩu.
* **Send Email:** Tích hợp dịch vụ gửi Email tự động từ hệ thống để cấp mã xác thực hoặc liên kết đặt lại mật khẩu bảo mật.

### 3. Quản Lý Đa Phương Tiện
* **Image Management / Processing:** Bổ sung module xử lý hình ảnh, cho phép tối ưu hóa dung lượng, kiểm tra định dạng và quản lý ảnh tải lên cho các bài viết CMS.

---

## 📂 Cấu Trúc Thư Mục Dự Án

Dự án được phân chia rõ ràng thành các phân hệ chính sau:

```text
LOI_CMS/
├── CMS_Backend/      # ASP.NET Core Web API (Xử lý Logic, Email, Image API)
├── CMS_Data/         # Entity Framework Core, SQL Server Migrations & Models
├── cms.frontend/     # Ứng dụng ReactJS (Giao diện, Sticky Header, Instant Search)
├── public/           # Tài nguyên tĩnh toàn cục
└── src/              # Mã nguồn bổ trợ
