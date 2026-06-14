## API & AUTHENTICATION DEVELOPMENT (BUỔI 09)

Trong buổi 09, hệ thống CMS được nâng cấp mạnh mẽ về khả năng bảo mật, quản lý tài khoản người dùng và tối ưu hóa việc lưu trữ, hiển thị tài nguyên đa phương tiện (Media/Images) trên cả hai phía Backend và Frontend.

---

### 1. Kiến trúc luồng xử lý (Data Flow)

#### 🔐 Luồng quên mật khẩu & Gửi Email (Forgot Password Flow)
1. **Frontend:** Người dùng nhập Email tại giao diện Forgot Password $\rightarrow$ Gửi yêu cầu `POST` tới API.
2. **Backend (CMS_Backend):** * Tiếp nhận yêu cầu, gọi `CMS_Data` kiểm tra Email có tồn tại trong Database không.
   * Nếu hợp lệ, hệ thống tự động sinh một mã mã hóa tạm thời (**Reset Token**) kèm thời gian hết hạn (Expiration Time).
   * Gọi `EmailService` cấu hình qua SMTP để gửi một email chứa đường dẫn khôi phục (hoặc mã OTP) về hòm thư của người dùng.
3. **Xác nhận:** Người dùng click vào link trong email $\rightarrow$ Frontend chuyển hướng đến trang đặt lại mật khẩu $\rightarrow$ Gọi API cập nhật mật khẩu mới vào cơ sở dữ liệu.

#### 📁 Luồng xử lý và hiển thị hình ảnh (Image Processing Flow)
1. **Upload:** Client gửi dữ liệu dưới dạng `multipart/form-data` chứa file ảnh bài viết.
2. **Xử lý tại Backend:** * Kiểm tra tính hợp lệ của file (định dạng ảnh, dung lượng).
   * Đổi tên file (sử dụng GUID hoặc Timestamp) để tránh trùng lặp tệp tin trên máy chủ.
   * Lưu trữ tệp tin vào thư mục lưu trữ tài nguyên tĩnh (Static Files).
3. **Trả kết quả:** API trả về một đường dẫn URL động trực tiếp dẫn đến file ảnh vừa lưu thay vì chuỗi text tĩnh (Static string).

---

### 2. Chi tiết các nội dung đã thực hiện

#### 🔹 Phần Backend & Data (`CMS_Backend` & `CMS_Data`)
* **Identity & Security Extension:**
  * Bổ sung các thuộc tính và logic xử lý liên quan đến Tokens khôi phục mật khẩu.
  * Tích hợp cơ chế mã hóa mật khẩu bảo mật trước khi lưu hoặc cập nhật lại vào Database.
* **Xây dựng `EmailService`:**
  * Triển khai dịch vụ gửi thư dựa trên nền tảng SMTP Client (sử dụng cấu hình hệ thống hoặc thư viện bổ sung như `MailKit`).
  * Quản lý thông tin cấu hình nhạy cảm (`SMTP Server`, `Port`, `App Password`) một cách an toàn thông qua `appsettings.json`.
* **Cải tiến `PostsController` (Xử lý Image):**
  * Tích hợp thuộc tính nhận file hình ảnh khi tạo/sửa bài viết thông qua giao tiếp API.
  * Cấu hình Static Files Middleware trong `Program.cs` để cho phép truy cập trực tiếp vào các tài nguyên hình ảnh được lưu trữ thông qua URL đường dẫn ảo.

#### 🔹 Phần Frontend (`cms.frontend`)
* **Phát triển giao diện mới:**
  * Thiết kế trang **Forgot Password** (Nhập email nhận link/mã).
  * Thiết kế trang **Reset Password** (Nhập mật khẩu mới và xác nhận mật khẩu).
* **Xử lý gọi API & Hiển thị:**
  * Sử dụng các thư viện HTTP Client (như `Axios` hoặc `Fetch API`) để gửi payload dữ liệu và xử lý các trạng thái phản hồi từ Server (Thành công, Lỗi 404 Email không tồn tại, Lỗi Token hết hạn).
  * Cập nhật các component hiển thị danh sách bài viết: Ràng buộc URL hình ảnh động trả về từ API vào thuộc tính `src` của thẻ `<img>` giúp giao diện hiển thị ảnh thực tế từ database.

---

### 3. Công nghệ và Thư viện áp dụng

| Thành phần | Công nghệ / Thư viện sử dụng | Mục đích triển khai |
| :--- | :--- | :--- |
| **Backend** | ASP.NET Core Web API | Xây dựng các Endpoint xử lý logic Auth và Media |
| **Database** | Entity Framework Core & SQL Server | Cập nhật thông tin mật khẩu mới, lưu trữ đường dẫn ảnh |
| **Mailing** | System.Net.Mail / MailKit | Kết nối máy chủ SMTP và phân phối Email tự động |
| **Frontend** | ReactJS (Hooks, State Management) | Quản lý trạng thái form, tương tác API bất đồng bộ |

---

### 4. Hướng dẫn Kiểm thử (Testing Guide)

#### Kiểm tra chức năng Forgot Password
1. Sử dụng **Swagger UI** (`/swagger`) hoặc **Postman**.
2. Gửi một request `POST` đến endpoint Auth với body:
   ```json
   {
     "email": "email_test_cua_ban@gmail.com"
   }
