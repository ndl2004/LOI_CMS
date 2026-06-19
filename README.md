# LOI_CMS - Website bán mỹ phẩm và hệ thống quản trị nội dung

LOI_CMS là dự án website bán mỹ phẩm được xây dựng theo mô hình 3 tầng, gồm tầng dữ liệu, tầng backend ASP.NET Core MVC/Web API và tầng frontend ReactJS. Dự án phục vụ đầy đủ các nghiệp vụ cơ bản của một website thương mại điện tử nhỏ: quản lý sản phẩm, danh mục, bài viết, banner, khách hàng, giỏ hàng, đặt hàng, flash sale, gửi email và phân quyền quản trị.

## Thông tin sinh viên

- Họ tên: Nguyễn Đình Lợi
- MSSV: 2122110147
- Lớp: CCQ2211D
- Tên dự án: LOI_CMS
- Chủ đề: Website bán mỹ phẩm kết hợp CMS quản trị nội dung

## Cấu trúc solution

```txt
LOI_CMS/
├── CMS_Data/        # Tầng dữ liệu: Entity, DbContext, Migration
├── CMS_Backend/     # Tầng backend: ASP.NET Core MVC, Web API, Admin
├── cms.frontend/    # Tầng frontend: ReactJS
├── README.md
├── .gitignore
└── LOI_ASP.sln
```

Dự án được tổ chức đúng mô hình 3 phân tầng:

- `CMS_Data`: khai báo entity, `ApplicationDbContext`, migration và kết nối SQL Server bằng Entity Framework Core.
- `CMS_Backend`: xây dựng trang quản trị bằng ASP.NET Core MVC, cung cấp RESTful API cho frontend, xử lý đăng nhập, phân quyền, email và nghiệp vụ đơn hàng.
- `cms.frontend`: giao diện người dùng bằng ReactJS, gọi API từ backend thông qua Axios.

## Công nghệ sử dụng

- ASP.NET Core MVC / Web API (.NET 8)
- Entity Framework Core 8
- SQL Server
- ReactJS
- React Router DOM
- Axios
- Bootstrap / CSS custom
- CKEditor
- Swagger / Swashbuckle
- BCrypt.Net-Next
- MailKit
- Git / GitHub

## Các thực thể chính trong hệ thống

Dự án hiện có hơn 8 thực thể theo yêu cầu, bao gồm:

- `User`: tài khoản quản trị
- `Customer`: khách hàng frontend
- `Category`: danh mục bài viết
- `Post`: bài viết/cẩm nang làm đẹp
- `CategoryProduct`: danh mục sản phẩm
- `Product`: sản phẩm
- `Order`: đơn hàng
- `OrderDetail`: chi tiết đơn hàng
- `Advertisement`: banner quảng cáo trang chủ
- `FlashSale`: chương trình khuyến mãi theo thời gian
- `FlashSaleItem`: sản phẩm trong chương trình flash sale

## Chức năng đã hoàn thành

### 1. Quản trị Admin

- Đăng nhập admin bằng cookie authentication.
- Đăng xuất an toàn, giải phóng phiên đăng nhập.
- Sidebar quản trị trong `_LayoutAdmin.cshtml`.
- Hiển thị tên người dùng đăng nhập và vai trò trên giao diện admin.
- Khóa trang quản trị bằng `[Authorize]`.
- Phân quyền quản lý thành viên bằng `[Authorize(Roles = "Admin")]` trong `UserController`.
- CRUD danh mục bài viết.
- CRUD bài viết.
- CRUD user quản trị.
- CRUD danh mục sản phẩm.
- CRUD sản phẩm.
- CRUD khách hàng.
- CRUD đơn hàng.
- CRUD chi tiết đơn hàng.
- CRUD banner quảng cáo.
- Quản lý flash sale: thêm/sửa/xóa chương trình, thêm nhiều sản phẩm sale, cập nhật phần trăm giảm giá và số lượng sale.
- Phân trang danh sách sản phẩm và bài viết trong admin.
- Tích hợp CKEditor vào nội dung bài viết.
- Upload ảnh trực tiếp vào CKEditor và lưu nội dung bài viết dạng HTML.

### 2. Frontend ReactJS

- Trang chủ có header, top nav, search, banner, danh mục, sản phẩm nổi bật, flash sale, sản phẩm mới và bài viết.
- Header/top nav/search dùng sticky để đứng yên khi cuộn trang.
- Header có ô tìm kiếm, gõ từ khóa sẽ điều hướng sang trang kết quả tìm kiếm.
- Header có badge giỏ hàng tự cập nhật theo số lượng sản phẩm trong localStorage.
- Top nav có dropdown danh mục sản phẩm lấy dữ liệu thật từ API.
- Trang shop hiển thị danh mục sản phẩm, danh sách sản phẩm, phân trang.
- Trang shop có tìm kiếm live search và lọc giá bằng 2 ô giá thấp nhất/cao nhất.
- Có thể kết hợp tìm kiếm, danh mục và khoảng giá.
- Khi không có kết quả lọc/tìm kiếm, giao diện hiển thị empty state rõ ràng.
- Trang chi tiết sản phẩm hiển thị đầy đủ ảnh, tên, danh mục, giá, mô tả, tồn kho, đã bán.
- Trang chi tiết sản phẩm kiểm tra tồn kho và cảnh báo: `Số lượng sản phẩm trong kho không đủ!`.
- Trang giỏ hàng cho phép tăng/giảm số lượng, xóa sản phẩm, tính tổng tiền.
- Trang checkout bắt buộc nhập họ tên, email, số điện thoại và địa chỉ.
- Luồng đặt hàng gửi POST xuống backend, tạo đơn hàng và trừ tồn kho.
- Trang lịch sử mua hàng hiển thị thông tin đơn hàng, người nhận, địa chỉ, trạng thái và chi tiết sản phẩm.
- Trang blog có dropdown danh mục để lọc bài viết theo danh mục.
- Trang chi tiết bài viết hiển thị nội dung HTML bằng `dangerouslySetInnerHTML`.
- Trang forgot password cho khách hàng.
- Trang profile khách hàng.

### 3. Flash Sale

- Admin tạo chương trình flash sale với thời gian bắt đầu/kết thúc.
- Có thể thêm nhiều sản phẩm vào flash sale cùng lúc.
- Kiểm tra số lượng sale không vượt quá tồn kho sản phẩm.
- Không cho tạo các chương trình flash sale đang hoạt động bị trùng thời gian.
- Frontend chỉ hiển thị flash sale khi đến đúng thời gian hiệu lực.
- Home hiển thị tối đa 4 sản phẩm flash sale và có đường dẫn sang trang khuyến mãi.
- Trang khuyến mãi hiển thị danh sách sản phẩm flash sale.
- Sản phẩm hết lượt sale sẽ hiển thị trạng thái hết hàng/hết lượt và làm mờ giao diện.
- Hiển thị số lượng sale còn lại cho người dùng.
- Đơn hàng lưu snapshot giá gốc, giá đã giảm và trạng thái flash sale tại thời điểm mua.

### 4. Đơn hàng và email

- Checkout tạo đơn hàng mới trong database.
- Đơn hàng lưu thông tin người nhận, email, số điện thoại, địa chỉ, phí ship, tổng tiền và phương thức thanh toán.
- Chi tiết đơn hàng lưu snapshot tên sản phẩm, giá gốc, giá bán, phần trăm giảm giá và trạng thái flash sale.
- Backend kiểm tra tồn kho trước khi tạo đơn.
- Khi đặt hàng thành công, hệ thống trừ tồn kho sản phẩm.
- Admin có thể duyệt hoặc từ chối đơn hàng trong trạng thái đơn.
- Khi từ chối đơn, hệ thống hoàn lại tồn kho và số lượng flash sale đã bán nếu có.
- Có gửi email thông tin/trạng thái đơn hàng cho khách hàng.

### 5. Bảo mật

- User admin không lưu mật khẩu thô, mật khẩu được hash bằng BCrypt.
- Customer không lưu mật khẩu thô, mật khẩu được hash bằng BCrypt.
- Đăng ký customer kiểm tra email trùng trước khi lưu.
- Forgot password gửi OTP qua email.
- OTP đổi mật khẩu có thời gian hết hạn.
- Admin được bảo vệ bằng cookie authentication và `[Authorize]`.
- Quản lý user chỉ dành cho tài khoản có role `Admin`.

### 6. API phục vụ frontend

Các API chính đang được sử dụng:

| Nhóm | Method | Endpoint | Chức năng |
| --- | --- | --- | --- |
| Products | GET | `/api/Products` | Lấy danh sách sản phẩm |
| Products | GET | `/api/Products/{id}` | Lấy chi tiết sản phẩm |
| Products | GET | `/api/Products/category/{categoryProductId}` | Lọc sản phẩm theo danh mục |
| Products | GET | `/api/Products/search?keyword=` | Tìm kiếm sản phẩm |
| Products | GET | `/api/Products/filter` | Lọc theo từ khóa, giá, danh mục |
| CategoriesProducts | GET | `/api/CategoriesProducts` | Lấy danh mục sản phẩm |
| Posts | GET | `/api/Posts` | Lấy danh sách bài viết |
| Posts | GET | `/api/Posts/{id}` | Lấy chi tiết bài viết |
| Posts | GET | `/api/Posts/category/{categoryId}` | Lọc bài viết theo danh mục |
| Categories | GET | `/api/Categories` | Lấy danh mục bài viết |
| Advertisements | GET | `/api/Advertisements` | Lấy banner trang chủ |
| FlashSales | GET | `/api/FlashSales/active` | Lấy flash sale đang hoạt động |
| Orders | POST | `/api/Orders` | Tạo đơn hàng |
| Orders | GET | `/api/Orders/customer/{customerId}` | Lịch sử mua hàng |
| Auth | POST | `/api/Auth/CustomerRegister` | Đăng ký khách hàng |
| Auth | POST | `/api/Auth/CustomerLogin` | Đăng nhập khách hàng |
| Auth | POST | `/api/Auth/SendForgotPasswordOtp` | Gửi OTP quên mật khẩu |
| Auth | POST | `/api/Auth/ResetPasswordWithOtp` | Đặt lại mật khẩu |
| Auth | PUT | `/api/Auth/UpdateProfile/{id}` | Cập nhật hồ sơ khách hàng |

Ví dụ JSON tạo đơn hàng:

```json
{
  "customerId": 1,
  "fullName": "Nguyen Van A",
  "email": "customer@example.com",
  "phone": "0900000000",
  "address": "TP. Ho Chi Minh",
  "items": [
    {
      "productId": 1,
      "quantity": 2
    }
  ]
}
```

## Cấu hình chạy dự án

### 1. Yêu cầu môi trường

- Visual Studio 2022
- .NET SDK 8
- SQL Server
- Node.js và npm
- Git

### 2. Clone source

```bash
git clone <repository-url>
cd LOI_CMS
```

### 3. Cấu hình database backend

Mở file:

```txt
CMS_Backend/appsettings.json
```

Cấu hình connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=LOI_CMS_DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

Nếu SQL Server của máy dùng tên server khác, thay `Server=.` bằng server phù hợp, ví dụ:

```txt
Server=localhost
Server=DESKTOP-xxx\\SQLEXPRESS
```

### 4. Chạy migration tạo database

Cách 1: dùng Package Manager Console trong Visual Studio:

```powershell
Update-Database -Project CMS_Data -StartupProject CMS_Backend
```

Cách 2: dùng terminal:

```bash
dotnet ef database update --project CMS_Data --startup-project CMS_Backend
```

### 5. Cấu hình email

Trong `CMS_Backend/appsettings.json`, cấu hình:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "SenderName": "LOI Cosmetics",
    "SenderEmail": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

Lưu ý: không nên public mật khẩu email thật lên GitHub. Nên dùng App Password của Gmail và thay lại trước khi demo.

### 6. Chạy backend

Mở solution `LOI_ASP.sln` bằng Visual Studio.

Chọn project startup:

```txt
CMS_Backend
```

Nhấn:

```txt
F5
```

Backend mặc định chạy tại:

```txt
https://localhost:7175
```

Swagger:

```txt
https://localhost:7175/swagger
```

Admin:

```txt
https://localhost:7175/Account/Login
```

### 7. Cấu hình frontend

Mở file:

```txt
cms.frontend/.env
```

Cấu hình:

```env
REACT_APP_API_URL=https://localhost:7175/api
REACT_APP_IMAGE_URL=https://localhost:7175
```

Nếu backend chạy port khác, sửa lại port tương ứng.

### 8. Chạy frontend

Mở terminal trong thư mục:

```bash
cd cms.frontend
```

Cài package:

```bash
npm install
```

Chạy React:

```bash
npm start
```

Frontend mặc định chạy tại:

```txt
http://localhost:3000
```

## Tài khoản demo

Tạo tài khoản admin trong bảng `Users` hoặc thông qua trang quản lý user. Tài khoản admin cần có:

```txt
Role = Admin
```

Ví dụ thông tin nên chuẩn bị trước khi demo:

```txt
Username: admin_loi1
Password: 123456
Role: Admin
```

Lưu ý: mật khẩu trong database phải được hash bằng BCrypt. Nếu tạo trực tiếp trong SQL Server bằng text thường thì đăng nhập sẽ không đúng.

## Kiểm tra trước khi demo

Trước khi nộp hoặc demo, nên kiểm tra lần lượt:

- Backend chạy không lỗi.
- Database đã update migration mới nhất.
- Swagger mở được.
- Frontend gọi API không lỗi CORS.
- F12 Console không có lỗi đỏ liên quan API/CORS.
- Admin đăng nhập được.
- Tạo/sửa/xóa sản phẩm được.
- Upload ảnh sản phẩm và ảnh bài viết được.
- Tạo bài viết bằng CKEditor và ảnh trong nội dung hiển thị đúng.
- Header search hoạt động.
- Shop lọc giá, lọc danh mục và tìm kiếm hoạt động.
- Giỏ hàng tăng/giảm/xóa sản phẩm được.
- Checkout tạo đơn hàng và trừ tồn kho.
- Admin duyệt/từ chối đơn hàng được.
- Email gửi được nếu cấu hình SMTP đúng.
- Flash sale chỉ hiển thị trong thời gian hiệu lực.

## Đối chiếu yêu cầu báo cáo

Các yêu cầu đã đáp ứng trong source code:

- Solution 3 tầng: đã có.
- Git có nhiều commit theo tiến độ: đã có.
- `.gitignore` loại bỏ `node_modules`, `bin`, `obj`, `build`: đã có.
- Entity đủ tối thiểu 8 class: đã có hơn 8 class.
- DbContext và migration: đã có.
- Admin CRUD các bảng chính: đã có.
- Phân trang Product/Post: đã có.
- CKEditor và upload ảnh bài viết: đã có.
- `[Authorize]` bảo vệ admin: đã có.
- `[Authorize(Roles = "Admin")]` cho UserController: đã có.
- Layout admin có sidebar, FullName và Role: đã có.
- Login/logout admin: đã có.
- Web API GET/POST cho frontend: đã có.
- CORS `AllowReactApp`: đã có.
- Middleware MVC + API trong `Program.cs`: đã có.
- Trang chủ frontend chia component: đã có.
- Link header/top nav hoạt động: đã có.
- HeroBanner dạng slide lấy dữ liệu từ API: đã có.
- Product detail và blog detail: đã có.
- Cart tăng/giảm/xóa và tính tổng: đã có.
- Checkout validate form và tạo order: đã có.
- Gửi email đơn hàng/trạng thái: đã có.
- Hash mật khẩu User/Customer bằng BCrypt: đã có.
- Register kiểm tra email trùng: đã có.
- Home có sản phẩm mới, bán chạy, danh mục: đã có.
- Shop lọc giá, search, empty state: đã có.
- Header search và cart badge realtime: đã có.
- Blog detail render HTML CKEditor: đã có.
- `.env` cấu hình API/image URL: đã có.
- Forgot password: đã có.


## Kết luận

Dự án LOI_CMS đã hoàn thiện phần lớn các yêu cầu chức năng của một website bán mỹ phẩm kết hợp hệ thống quản trị nội dung. Dự án có đầy đủ backend MVC, Web API, frontend ReactJS, cơ sở dữ liệu SQL Server, bảo mật đăng nhập, phân quyền, giỏ hàng, đặt hàng, email, CKEditor, flash sale và các chức năng lọc/tìm kiếm phục vụ trải nghiệm người dùng.

## Kiến trúc hệ thống

Hệ thống được xây dựng theo mô hình 3 tầng (Three-Layer Architecture) nhằm đảm bảo tính tách biệt giữa giao diện, nghiệp vụ và dữ liệu.

```text
┌──────────────────────────┐
│      ReactJS Frontend    │
│  (Giao diện người dùng)  │
└────────────┬─────────────┘
             │ HTTP/Axios
             ▼
┌──────────────────────────┐
│ ASP.NET Core MVC/Web API │
│   (Business Logic Layer) │
└────────────┬─────────────┘
             │ EF Core
             ▼
┌──────────────────────────┐
│      SQL Server DB       │
│      (Data Layer)        │
└──────────────────────────┘
```

Người dùng tương tác với giao diện ReactJS. Frontend gửi yêu cầu đến ASP.NET Core Web API thông qua Axios. Backend xử lý nghiệp vụ, truy cập cơ sở dữ liệu thông qua Entity Framework Core và trả kết quả về cho frontend hiển thị.

---

## Luồng xử lý đặt hàng

Quy trình đặt hàng được triển khai theo các bước:

```text
Khách hàng
    │
    ▼
Thêm sản phẩm vào giỏ hàng
    │
    ▼
Checkout
    │
    ▼
API Orders
    │
    ├─ Kiểm tra tồn kho
    ├─ Tính tổng tiền
    ├─ Tạo Order
    ├─ Tạo OrderDetail
    ├─ Trừ tồn kho
    ├─ Cập nhật Flash Sale
    └─ Gửi Email xác nhận
    │
    ▼
Đặt hàng thành công
```

Hệ thống đảm bảo dữ liệu đơn hàng được lưu đầy đủ, đồng thời tự động cập nhật số lượng tồn kho và trạng thái Flash Sale.

---

## Thiết kế cơ sở dữ liệu

Các bảng dữ liệu chính trong hệ thống:

```text
Users
Customers
Categories
Posts
CategoryProducts
Products
Orders
OrderDetails
Advertisements
FlashSales
FlashSaleItems
```

Mối quan hệ dữ liệu:

```text
Customer
   │
   └── Orders
            │
            └── OrderDetails
                        │
                        └── Product

CategoryProduct
        │
        └── Products

Category
    │
    └── Posts

FlashSale
    │
    └── FlashSaleItems
                    │
                    └── Product
```

Hệ thống sử dụng Entity Framework Core Code First kết hợp Migration để quản lý và đồng bộ cơ sở dữ liệu.

---

## Các tính năng nổi bật

### Flash Sale theo thời gian thực

* Quản lý chương trình khuyến mãi theo thời gian.
* Không cho phép tạo Flash Sale bị trùng thời gian hiệu lực.
* Theo dõi số lượng sản phẩm được bán trong chương trình.
* Tự động hiển thị hoặc ẩn Flash Sale theo thời gian thực.
* Lưu snapshot giá tại thời điểm khách hàng mua hàng.

### CMS quản trị nội dung

* Quản lý bài viết và danh mục bài viết.
* Soạn thảo nội dung bằng CKEditor.
* Upload hình ảnh trực tiếp trong nội dung bài viết.
* Hiển thị nội dung HTML phía frontend.

### Hệ thống email tự động

Email được gửi trong các trường hợp:

* Gửi OTP quên mật khẩu.
* Xác nhận đặt hàng thành công.
* Thông báo thay đổi trạng thái đơn hàng.
* Thông báo hủy đơn hàng.

---

## Bảo mật hệ thống

Các cơ chế bảo mật đã được áp dụng:

* Mã hóa mật khẩu bằng BCrypt.
* Cookie Authentication cho khu vực quản trị.
* Authorize Attribute bảo vệ các trang quản trị.
* Role-Based Authorization cho tài khoản Admin.
* Xác thực OTP khi đổi mật khẩu.
* Kiểm tra dữ liệu đầu vào bằng Validation.
* Phân quyền chức năng theo vai trò người dùng.

---

## Kết quả đạt được

Sau quá trình xây dựng và hoàn thiện, hệ thống đã đáp ứng các chức năng chính của một website thương mại điện tử kết hợp CMS:

✅ Quản lý sản phẩm

✅ Quản lý bài viết

✅ Quản lý khách hàng

✅ Quản lý đơn hàng

✅ Flash Sale

✅ Gửi Email tự động

✅ Đăng ký và đăng nhập khách hàng

✅ Quên mật khẩu bằng OTP

✅ Phân quyền quản trị

✅ Giỏ hàng và đặt hàng

✅ Tìm kiếm và lọc sản phẩm

✅ Website ReactJS kết nối ASP.NET Core Web API

---

## Hướng phát triển

Trong tương lai, hệ thống có thể được mở rộng thêm các chức năng:

* Tích hợp thanh toán trực tuyến VNPay.
* Tích hợp thanh toán MoMo.
* Đăng nhập bằng Google OAuth.
* Tích hợp Redis Cache để tăng hiệu năng.
* Dashboard thống kê doanh thu bằng biểu đồ.
* Tích hợp Elasticsearch cho tìm kiếm nâng cao.
* Đóng gói bằng Docker và triển khai Cloud.
* Xây dựng ứng dụng Mobile App sử dụng chung API.

