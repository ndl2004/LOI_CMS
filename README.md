# FRONTEND DEVELOPMENT & INTEGRATION (BUỔI 07)

Trong buổi 07, dự án tập trung vào việc xây dựng giao diện người dùng (Frontend) bằng ReactJS (`cms.frontend`) và thực hiện kết nối, gọi các API cơ bản từ Backend để hiển thị dữ liệu lên giao diện.

---

## 🚀 Nội dung đã thực hiện

### 1. Xây dựng Giao diện Frontend (ReactJS)
* Khởi tạo và cấu hình cấu trúc thư mục ứng dụng trong `cms.frontend`.
* Thiết kế giao diện các trang chức năng chính đảm bảo tính responsive và thân thiện với người dùng.
* Xây dựng hệ thống Routing để điều hướng mượt mà giữa các trang.

### 2. Tích hợp API & Hiển thị Dữ liệu
Sử dụng `fetch` hoặc `axios` để kết nối và lấy dữ liệu dạng JSON từ ASP.NET Core Web API:
* **Danh mục (Category):** Lấy danh sách danh mục để đổ vào Menu/Sidebar.
* **Sản phẩm (Product):** Hiển thị danh sách sản phẩm, chi tiết sản phẩm.
* **Bài viết (Post):** Gọi API `GET /api/posts` để hiển thị danh sách bài viết mới nhất lên giao diện UI.
* **Đăng nhập (Login):** Xây dựng form đăng nhập, xử lý gửi dữ liệu tài khoản lên Backend để xác thực.

### 3. Quản lý Trạng thái & Xử lý Logic
* Sử dụng `useState` và `useEffect` trong React để quản lý vòng đời component và trạng thái dữ liệu khi gọi API.
* Xử lý trạng thái Loading (đang tải dữ liệu) và thông báo lỗi trực quan nếu API gặp sự cố.

---

## 🛠️ Công nghệ sử dụng trong buổi này

* **Frontend Framework:** ReactJS (Hooks, React Router DOM)
* **Styling:** CSS / HTML5 (hoặc TailwindCSS/Bootstrap tùy theo UI bạn dùng)
* **API Client:** Axios / Fetch API
* **Backend hỗ trợ:** ASP.NET Core Web API, Entity Framework Core (từ Buổi 06)

---

## 📂 Cấu trúc thư mục cập nhật

* `cms.frontend/`: Chứa toàn bộ mã nguồn giao diện ReactJS.
* `CMS_Backend/`: API RESTful phục vụ dữ liệu cho Frontend.
* `CMS_Data/`: Lớp kết nối và quản lý cơ sở dữ liệu SQL Server.

---

## 🏃‍♂️ Hướng dẫn chạy dự án

### Khởi chạy Backend
1. Mở file `LOI_ASP.sln` bằng Visual Studio.
2. Nhấn `F5` hoặc nút `Start` để chạy Backend (mặc định sẽ mở Swagger tại `/swagger`).

### Khởi chạy Frontend
1. Mở thư mục `cms.frontend` bằng terminal.
2. Cài đặt dependencies (nếu có thêm thư viện mới):
   ```bash
   npm install
