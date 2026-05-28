# LOI_CMS — Buổi 05

## Thông tin sinh viên

* Họ tên: Nguyễn Đình Lợi
* MSSV: 2122110147
* Lớp: CCQ2211D
* Môn học: Chuyên đề ASP.NET
* Tên project: LOI_CMS

---

## Giới thiệu dự án

LOI_CMS là project xây dựng hệ thống quản lý nội dung sử dụng ASP.NET Core MVC, Entity Framework Core và SQL Server.

Ở nhánh `buoi05`, project đã phát triển từ phần quản lý nội dung cơ bản sang hệ thống CMS có nhiều module hơn, bao gồm:

* Quản lý danh mục bài viết
* Quản lý bài viết
* Quản lý người dùng
* Đăng nhập, đăng xuất bằng Cookie Authentication
* Phân quyền truy cập bằng Role
* Quản lý danh mục sản phẩm
* Quản lý sản phẩm
* Quản lý khách hàng
* Quản lý đơn hàng
* Quản lý chi tiết đơn hàng
* Chuẩn bị frontend ReactJS để kết nối API trong các buổi tiếp theo

---

## Công nghệ sử dụng

### Backend

* ASP.NET Core MVC
* Entity Framework Core
* SQL Server
* Cookie Authentication
* Razor View
* Bootstrap
* LINQ

### Frontend

* ReactJS
* React DOM
* React Scripts
* JavaScript
* HTML
* CSS

### Database

* SQL Server
* Entity Framework Core Migration
* DbContext
* DbSet

---

## Cấu trúc thư mục chính

```txt
LOI_CMS
│
├── CMS_Backend
│   ├── Controllers
│   ├── Models
│   ├── Views
│   ├── wwwroot
│   ├── Program.cs
│   └── appsettings.json
│
├── CMS_Data
│   ├── Entities
│   ├── Migrations
│   ├── ApplicationDbContext.cs
│   └── ApplicationDbContextFactory.cs
│
├── cms.frontend
│
├── public
│
├── src
│
├── package.json
│
└── LOI_ASP.sln
```

---

## Các project chính

### 1. CMS_Backend

Đây là project ASP.NET Core MVC chính, dùng để xây dựng trang quản trị Admin.

Project này chứa:

* Controller xử lý chức năng
* View hiển thị giao diện
* Layout trang quản trị
* Cấu hình xác thực đăng nhập
* Cấu hình kết nối database
* Cấu hình route MVC

Các controller chính gồm:

```txt
AccountController.cs
CategoryController.cs
CategoryProductController.cs
Customers.cs
HomeController.cs
OrderController.cs
OrderDetailController.cs
PostController.cs
Products.cs
UserController.cs
```

---

### 2. CMS_Data

Đây là project chứa tầng dữ liệu của hệ thống.

Project này dùng để:

* Khai báo Entity
* Khai báo DbContext
* Quản lý Migration
* Kết nối Entity Framework Core với SQL Server

Các entity chính gồm:

```txt
Category.cs
CategoryProduct.cs
Customer.cs
Order.cs
OrderDetail.cs
Post.cs
Product.cs
User.cs
```

---

### 3. cms.frontend

Đây là phần frontend ReactJS được chuẩn bị để phát triển giao diện người dùng hoặc giao diện gọi API ở các buổi sau.

Các package React đã được cấu hình trong `package.json`, bao gồm:

* react
* react-dom
* react-scripts
* web-vitals
* testing-library

---

## Nội dung đã thực hiện trong buổi 05

## 1. Cấu hình kết nối database

Trong `Program.cs`, project đã đăng ký `ApplicationDbContext` vào hệ thống bằng Dependency Injection.

Mục đích:

* Cho phép các controller sử dụng database
* Kết nối SQL Server thông qua connection string
* Làm việc với Entity Framework Core

Ví dụ chức năng đã cấu hình:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

---

## 2. Xây dựng ApplicationDbContext

File `ApplicationDbContext.cs` được dùng để đại diện cho database trong Entity Framework Core.

Trong DbContext đã khai báo các bảng chính:

```csharp
public DbSet<Category> Categories { get; set; }
public DbSet<Post> Posts { get; set; }
public DbSet<User> Users { get; set; }
public DbSet<CategoryProduct> CategoriesProducts { get; set; }
public DbSet<Product> Products { get; set; }
public DbSet<Customer> Customers { get; set; }
public DbSet<Order> Orders { get; set; }
public DbSet<OrderDetail> OrderDetails { get; set; }
```

Nhờ đó, hệ thống có thể thao tác dữ liệu với các bảng như bài viết, danh mục, sản phẩm, đơn hàng, khách hàng và người dùng.

---

## 3. Xây dựng chức năng đăng nhập

Project đã có `AccountController` để xử lý chức năng đăng nhập và đăng xuất.

Chức năng chính:

* Hiển thị form đăng nhập
* Kiểm tra thông tin tài khoản
* Tạo cookie đăng nhập
* Lưu thông tin người dùng vào Claim
* Đăng xuất khỏi hệ thống
* Chuyển hướng khi không có quyền truy cập

---

## 4. Cấu hình Cookie Authentication

Trong `Program.cs`, hệ thống đã cấu hình xác thực bằng Cookie.

Mục đích:

* Kiểm tra người dùng đã đăng nhập hay chưa
* Chuyển về trang Login nếu chưa đăng nhập
* Chuyển đến trang AccessDenied nếu không đủ quyền

Cấu hình chính:

```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
```

---

## 5. Cấu hình Middleware Authentication và Authorization

Project đã thêm middleware xác thực và phân quyền.

Thứ tự middleware:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

Ý nghĩa:

* `UseAuthentication()` kiểm tra người dùng là ai
* `UseAuthorization()` kiểm tra người dùng có quyền làm gì

Đây là phần quan trọng để bảo vệ các trang quản trị.

---

## 6. Xây dựng phân quyền người dùng

Hệ thống có sử dụng Role để phân quyền truy cập.

Ví dụ:

* Admin có quyền quản lý thành viên
* Editor có thể đăng nhập nhưng không được truy cập một số chức năng quản trị quan trọng

Khi người dùng không có quyền, hệ thống chuyển về trang thông báo không có quyền truy cập.

---

## 7. Xây dựng giao diện Admin Layout

Project đã có layout quản trị dùng chung cho các trang admin.

Layout có các phần:

* Sidebar menu
* Tên hệ thống ThaiCMS Admin
* Hiển thị tên người đăng nhập
* Hiển thị quyền của người đăng nhập
* Nút đăng xuất
* Menu điều hướng đến các module quản lý

Các menu chính:

```txt
Bảng điều khiển
Quản lý Danh mục
Quản lý Bài viết
Quản lý Thành viên
Danh mục sản phẩm
Sản phẩm
Khách hàng
Đơn hàng
Đơn hàng chi tiết
```

---

## 8. Module quản lý danh mục bài viết

Module danh mục bài viết dùng để quản lý các nhóm bài viết trong CMS.

Chức năng chính:

* Hiển thị danh sách danh mục
* Thêm danh mục mới
* Sửa thông tin danh mục
* Xóa danh mục
* Kết nối danh mục với bài viết

Entity liên quan:

```csharp
Category
```

Controller liên quan:

```txt
CategoryController.cs
```

---

## 9. Module quản lý bài viết

Module bài viết dùng để quản lý nội dung trong hệ thống CMS.

Chức năng chính:

* Hiển thị danh sách bài viết
* Thêm bài viết
* Sửa bài viết
* Xóa bài viết
* Gắn bài viết với danh mục
* Hiển thị tiêu đề, nội dung, hình ảnh và ngày tạo

Entity liên quan:

```csharp
Post
```

Controller liên quan:

```txt
PostController.cs
```

---

## 10. Module quản lý thành viên

Module quản lý thành viên dùng để quản lý tài khoản đăng nhập vào hệ thống.

Chức năng chính:

* Hiển thị danh sách user
* Thêm user mới
* Sửa thông tin user
* Đổi mật khẩu
* Xóa user
* Kiểm tra username bị trùng
* Phân quyền user theo Role

Entity liên quan:

```csharp
User
```

Controller liên quan:

```txt
UserController.cs
```

---

## 11. Module quản lý danh mục sản phẩm

Module danh mục sản phẩm dùng để phân loại sản phẩm.

Chức năng chính:

* Hiển thị danh sách danh mục sản phẩm
* Thêm danh mục sản phẩm
* Sửa danh mục sản phẩm
* Xóa danh mục sản phẩm

Entity liên quan:

```csharp
CategoryProduct
```

Controller liên quan:

```txt
CategoryProductController.cs
```

---

## 12. Module quản lý sản phẩm

Module sản phẩm dùng để quản lý thông tin sản phẩm trong hệ thống.

Chức năng chính:

* Hiển thị danh sách sản phẩm
* Thêm sản phẩm
* Sửa sản phẩm
* Xóa sản phẩm
* Liên kết sản phẩm với danh mục sản phẩm

Entity liên quan:

```csharp
Product
```

Controller liên quan:

```txt
Products.cs
```

---

## 13. Module quản lý khách hàng

Module khách hàng dùng để lưu trữ và quản lý thông tin khách hàng.

Chức năng chính:

* Hiển thị danh sách khách hàng
* Thêm khách hàng
* Sửa thông tin khách hàng
* Xóa khách hàng

Entity liên quan:

```csharp
Customer
```

Controller liên quan:

```txt
Customers.cs
```

---

## 14. Module quản lý đơn hàng

Module đơn hàng dùng để quản lý các đơn đặt hàng của khách hàng.

Chức năng chính:

* Hiển thị danh sách đơn hàng
* Thêm đơn hàng
* Sửa đơn hàng
* Xóa đơn hàng
* Liên kết đơn hàng với khách hàng

Entity liên quan:

```csharp
Order
```

Controller liên quan:

```txt
OrderController.cs
```

---

## 15. Module quản lý chi tiết đơn hàng

Module chi tiết đơn hàng dùng để lưu thông tin sản phẩm nằm trong từng đơn hàng.

Chức năng chính:

* Hiển thị chi tiết đơn hàng
* Thêm chi tiết đơn hàng
* Sửa chi tiết đơn hàng
* Xóa chi tiết đơn hàng
* Liên kết đơn hàng với sản phẩm

Entity liên quan:

```csharp
OrderDetail
```

Controller liên quan:

```txt
OrderDetailController.cs
```

---

## 16. Sử dụng Entity Framework Core Migration

Project có thư mục `Migrations` trong `CMS_Data`.

Mục đích:

* Quản lý thay đổi cấu trúc database
* Tạo bảng từ các entity
* Cập nhật database khi entity thay đổi

Các lệnh thường dùng:

```powershell
Add-Migration TenMigration
Update-Database
```

---

## 17. Chuẩn bị ReactJS Frontend

Trong nhánh `buoi05`, project đã có cấu hình ReactJS.

Mục đích:

* Chuẩn bị giao diện frontend riêng
* Sau này có thể gọi API từ backend
* Tách phần giao diện người dùng khỏi trang Admin MVC

Các lệnh React cơ bản:

```bash
npm install
npm start
npm run build
```

---

## Cách chạy project

### 1. Clone project

```bash
git clone https://github.com/ndl2004/LOI_CMS.git
```

### 2. Chuyển sang nhánh buoi05

```bash
git checkout buoi05
```

### 3. Mở solution

Mở file:

```txt
LOI_ASP.sln
```

bằng Visual Studio.

### 4. Kiểm tra connection string

Mở file:

```txt
CMS_Backend/appsettings.json
```

Kiểm tra cấu hình:

```json
"ConnectionStrings": {
  "DefaultConnection": "..."
}
```

### 5. Cập nhật database

Trong Package Manager Console:

```powershell
Update-Database
```

### 6. Chạy project

Bấm:

```txt
F5
```

hoặc:

```txt
Ctrl + F5
```

---

## Một số đường dẫn chức năng

```txt
/Account/Login
/Account/Logout
/Account/AccessDenied

/Category
/Post
/User
/ProductCategory
/Product
/Customer
/Order
/OrderDetail
```

---

## Kết quả đạt được

Sau buổi 05, project đã đạt được các nội dung sau:

* Xây dựng được project ASP.NET Core MVC theo mô hình nhiều tầng
* Tách riêng tầng backend và tầng data
* Kết nối SQL Server bằng Entity Framework Core
* Tạo được nhiều entity cho hệ thống CMS và bán hàng
* Tạo được DbContext quản lý nhiều bảng dữ liệu
* Xây dựng được các controller quản lý dữ liệu
* Xây dựng được layout trang Admin
* Thêm chức năng đăng nhập và đăng xuất
* Cấu hình Cookie Authentication
* Cấu hình phân quyền bằng Role
* Bảo vệ các trang quản trị bằng Authorize
* Xây dựng các module quản lý bài viết, danh mục, người dùng, sản phẩm, khách hàng và đơn hàng
* Chuẩn bị frontend ReactJS cho các phần phát triển tiếp theo

---

## Kiến thức đã học được

Thông qua buổi 05, sinh viên đã thực hành được:

* Cách tổ chức solution ASP.NET Core
* Cách tách project Backend và Data
* Cách khai báo Entity
* Cách sử dụng DbContext
* Cách dùng DbSet để ánh xạ bảng dữ liệu
* Cách kết nối SQL Server
* Cách dùng Entity Framework Core
* Cách tạo controller MVC
* Cách tạo Razor View
* Cách dùng Bootstrap để xây dựng giao diện quản trị
* Cách đăng nhập bằng Cookie Authentication
* Cách phân quyền bằng Role
* Cách dùng `[Authorize]`
* Cách xử lý trang Access Denied
* Cách chuẩn bị ReactJS frontend

---

## Định hướng phát triển tiếp theo

Các phần có thể phát triển trong những buổi sau:

* Bổ sung Swagger để kiểm thử API
* Xây dựng API Controller cho bài viết, sản phẩm, đơn hàng
* Kết nối ReactJS với ASP.NET Core API
* Hoàn thiện giao diện frontend
* Hash mật khẩu thay vì lưu mật khẩu dạng thường
* Tối ưu phân quyền Admin, Editor, User
* Thêm upload hình ảnh
* Thêm tìm kiếm, phân trang, lọc dữ liệu
* Cải thiện giao diện quản trị
* Tối ưu database relationship

---

## Ghi chú

Đây là project học tập phục vụ môn Chuyên đề ASP.NET. Project được xây dựng từng bước qua các buổi học, trong đó nhánh `buoi05` tập trung vào việc mở rộng hệ thống CMS, bổ sung xác thực đăng nhập, phân quyền và các module quản lý dữ liệu quan trọng.
