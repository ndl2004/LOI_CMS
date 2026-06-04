# BUỔI 06 - XÂY DỰNG WEB API CHO FRONTEND REACTJS

## 1. Thông tin sinh viên

| Thông tin          | Nội dung                                  |
| ------------------ | ----------------------------------------- |
| Họ tên             | Nguyễn Đình Lợi                           |
| MSSV               | 2122110147                                |
| Lớp                | CCQ2211D                                  |
| Môn học            | Chuyên đề ASP.NET                         |
| Tên dự án          | LOI_CMS                                   |
| Nội dung thực hiện | Xây dựng Web API phục vụ Frontend ReactJS |

---

## 2. Mục tiêu buổi 06

Buổi 06 tập trung xây dựng các Web API trong ASP.NET Core để cung cấp dữ liệu cho Frontend ReactJS.

Các API được thiết kế theo chuẩn RESTful, dữ liệu trao đổi giữa Backend và Frontend sử dụng định dạng JSON.

Mục tiêu chính:

* Xây dựng API lấy dữ liệu danh mục sản phẩm.
* Xây dựng API lấy dữ liệu bài viết.
* Xây dựng API lấy dữ liệu sản phẩm.
* Xây dựng API đăng ký và đăng nhập khách hàng.
* Xây dựng API xử lý đặt hàng.
* Xây dựng API tra cứu lịch sử đơn hàng của khách hàng.
* Kiểm thử toàn bộ API bằng Swagger UI.

---

## 3. Công nghệ sử dụng

| Công nghệ             | Mục đích sử dụng                      |
| --------------------- | ------------------------------------- |
| ASP.NET Core MVC      | Xây dựng hệ thống quản trị Admin      |
| ASP.NET Core Web API  | Cung cấp dữ liệu cho Frontend ReactJS |
| Entity Framework Core | Làm việc với cơ sở dữ liệu            |
| SQL Server            | Lưu trữ dữ liệu hệ thống              |
| Swagger UI            | Kiểm thử API                          |
| Visual Studio 2022    | Môi trường phát triển                 |
| GitHub                | Quản lý mã nguồn                      |

---

## 4. Cấu trúc thư mục chính

```txt
LOI_CMS
│
├── CMS_Backend
│   ├── Controllers
│   │   ├── API
│   │   │   ├── AuthController.cs
│   │   │   ├── CategoriesProductsController.cs
│   │   │   ├── OrdersController.cs
│   │   │   ├── PostsController.cs
│   │   │   └── ProductsController.cs
│   │   │
│   │   ├── AccountController.cs
│   │   ├── CategoryController.cs
│   │   ├── CategoryProductController.cs
│   │   ├── CustomersController.cs
│   │   ├── OrderController.cs
│   │   ├── OrderDetailController.cs
│   │   ├── PostController.cs
│   │   ├── ProductsController.cs
│   │   └── UserController.cs
│   │
│   ├── Views
│   ├── wwwroot
│   ├── appsettings.json
│   └── Program.cs
│
└── CMS_Data
    ├── Entities
    │   ├── Category.cs
    │   ├── CategoryProduct.cs
    │   ├── Customer.cs
    │   ├── Order.cs
    │   ├── OrderDetail.cs
    │   ├── Post.cs
    │   ├── Product.cs
    │   └── User.cs
    │
    └── ApplicationDbContext.cs
```

---

## 5. Các chức năng đã thực hiện

## 5.1. API danh mục sản phẩm

### Endpoint

```http
GET /api/CategoriesProducts
```

### Mục đích

API này dùng để lấy toàn bộ danh sách danh mục sản phẩm từ bảng `CategoryProduct`.

### Ứng dụng trên Frontend

Dữ liệu API này có thể được sử dụng để hiển thị menu danh mục sản phẩm trong ReactJS, ví dụ:

* Menu danh mục ngang ở trang chủ.
* Bộ lọc sản phẩm theo danh mục ở trang Shop.
* Danh sách loại sản phẩm trong giao diện người dùng.

### Dữ liệu trả về mẫu

```json
[
  {
    "id": 1,
    "name": "Áo Nam",
    "description": "Danh mục áo nam"
  },
  {
    "id": 2,
    "name": "Quần Nam",
    "description": "Danh mục quần nam"
  }
]
```

---

## 5.2. API bài viết

### Endpoint

```http
GET /api/Posts
GET /api/Posts/{id}
GET /api/Posts/category/{categoryId}
```

### Chức năng

API bài viết dùng để cung cấp dữ liệu tin tức cho Frontend.

Các chức năng gồm:

* Lấy toàn bộ danh sách bài viết.
* Lấy bài viết theo danh mục.
* Lấy chi tiết bài viết theo ID.
* Trả dữ liệu dưới dạng JSON.

### Ứng dụng trên Frontend

API này có thể dùng cho:

* Khu vực tin tức ở trang chủ.
* Trang danh sách bài viết.
* Trang chi tiết bài viết.
* Khối bài viết mới nhất hoặc bài viết theo danh mục.

### Dữ liệu trả về mẫu

```json
{
  "id": 1,
  "title": "Xu hướng thời trang mới",
  "imageUrl": "/uploads/posts/news.jpg",
  "createdDate": "2026-06-04T00:00:00",
  "categoryName": "Thời trang"
}
```

---

## 5.3. API sản phẩm

### Endpoint

```http
GET /api/Products
GET /api/Products/{id}
GET /api/Products/category/{categoryProductId}
```

### Chức năng

API sản phẩm dùng để cung cấp dữ liệu sản phẩm cho giao diện ReactJS.

Các chức năng gồm:

* Lấy toàn bộ danh sách sản phẩm.
* Lấy sản phẩm theo danh mục sản phẩm.
* Lấy chi tiết sản phẩm theo ID.
* Trả về các thông tin quan trọng như tên, giá, ảnh, mô tả, tồn kho và danh mục.

### Ứng dụng trên Frontend

API này có thể dùng cho:

* Trang chủ `Home.jsx`.
* Trang cửa hàng `Shop.jsx`.
* Trang chi tiết sản phẩm `ProductDetail.jsx`.
* Lưới sản phẩm.
* Bộ lọc sản phẩm theo danh mục.

### Dữ liệu danh sách sản phẩm trả về mẫu

```json
[
  {
    "id": 1,
    "name": "Áo sơ mi nam",
    "price": 250000,
    "imageUrl": "/uploads/products/aosomi.jpg",
    "categoryProductId": 1
  }
]
```

### Dữ liệu chi tiết sản phẩm trả về mẫu

```json
{
  "id": 1,
  "name": "Áo sơ mi nam",
  "description": "Áo sơ mi nam chất liệu cotton",
  "price": 250000,
  "stockQuantity": 10,
  "imageUrl": "/uploads/products/aosomi.jpg",
  "categoryProductId": 1,
  "categoryName": "Áo Nam"
}
```

---

## 5.4. API đăng ký khách hàng

### Endpoint

```http
POST /api/Auth/CustomerRegister
```

### Chức năng

API này dùng để đăng ký tài khoản khách hàng mới.

Quy trình xử lý:

1. Frontend gửi thông tin khách hàng lên Backend.
2. Backend kiểm tra Email đã tồn tại hay chưa.
3. Nếu Email chưa tồn tại, hệ thống thêm khách hàng mới vào bảng `Customers`.
4. Backend trả về thông báo đăng ký thành công.

### Dữ liệu gửi lên mẫu

```json
{
  "fullName": "Nguyen Van A",
  "email": "a@gmail.com",
  "password": "123456",
  "phone": "0909123456",
  "address": "Thu Duc"
}
```

### Dữ liệu trả về mẫu

```json
{
  "message": "Đăng ký tài khoản thành công",
  "id": 5,
  "fullName": "Nguyen Van A",
  "email": "a@gmail.com",
  "phone": "0909123456",
  "address": "Thu Duc"
}
```

---

## 5.5. API đăng nhập khách hàng

### Endpoint

```http
POST /api/Auth/CustomerLogin
```

### Chức năng

API này dùng để kiểm tra thông tin đăng nhập của khách hàng.

Quy trình xử lý:

1. Frontend gửi Email và Password lên Backend.
2. Backend kiểm tra thông tin trong bảng `Customers`.
3. Nếu đúng, hệ thống trả về thông tin khách hàng.
4. Nếu sai, hệ thống trả về lỗi đăng nhập.

### Dữ liệu gửi lên mẫu

```json
{
  "email": "a@gmail.com",
  "password": "123456"
}
```

### Dữ liệu trả về mẫu

```json
{
  "message": "Đăng nhập thành công",
  "id": 5,
  "fullName": "Nguyen Van A",
  "email": "a@gmail.com",
  "phone": "0909123456",
  "address": "Thu Duc"
}
```

---

## 5.6. API đặt hàng

### Endpoint

```http
POST /api/Orders
```

### Chức năng

API này là chức năng chính của luồng đặt hàng từ Frontend.

Quy trình xử lý:

1. Frontend gửi thông tin khách hàng và danh sách sản phẩm trong giỏ hàng.
2. Backend kiểm tra khách hàng có tồn tại hay không.
3. Backend kiểm tra giỏ hàng có sản phẩm hay không.
4. Backend tạo đơn hàng mới trong bảng `Orders`.
5. Backend duyệt từng sản phẩm trong giỏ hàng.
6. Backend kiểm tra sản phẩm có tồn tại hay không.
7. Backend kiểm tra số lượng tồn kho.
8. Backend thêm dữ liệu vào bảng `OrderDetails`.
9. Backend khấu trừ `StockQuantity` của sản phẩm.
10. Backend lưu thay đổi vào SQL Server.
11. Backend trả về thông báo đặt hàng thành công.

### Dữ liệu gửi lên mẫu

```json
{
  "customerId": 5,
  "notes": "Giao giờ hành chính",
  "items": [
    {
      "productId": 1,
      "quantity": 2
    },
    {
      "productId": 3,
      "quantity": 1
    }
  ]
}
```

### Dữ liệu trả về mẫu

```json
{
  "message": "Đặt hàng thành công",
  "orderId": 6,
  "customerId": 5,
  "orderDate": "2026-06-04T14:09:05.5391185",
  "status": 0,
  "totalAmount": 450000
}
```

### Ghi chú

Trạng thái đơn hàng mặc định:

| Status | Ý nghĩa       |
| ------ | ------------- |
| 0      | Chờ duyệt     |
| 1      | Đang giao     |
| 2      | Đã hoàn thành |

---

## 5.7. API lịch sử đơn hàng theo khách hàng

### Endpoint

```http
GET /api/Orders/customer/{customerId}
```

### Chức năng

API này dùng để lấy danh sách đơn hàng của một khách hàng cụ thể.

Dữ liệu trả về gồm:

* Mã đơn hàng.
* Ngày đặt hàng.
* Trạng thái đơn hàng.
* Ghi chú của khách hàng.
* Danh sách sản phẩm trong đơn hàng.
* Số lượng sản phẩm.
* Đơn giá.
* Thành tiền từng sản phẩm.
* Tổng tiền đơn hàng.

### Ứng dụng trên Frontend

API này có thể dùng cho:

* Trang thông tin cá nhân khách hàng.
* Trang lịch sử mua hàng.
* Trang theo dõi đơn hàng.

### Dữ liệu trả về mẫu

```json
[
  {
    "id": 6,
    "orderDate": "2026-06-04T14:09:05.5391185",
    "status": 0,
    "notes": "Giao giờ hành chính",
    "details": [
      {
        "productId": 1,
        "productName": "Áo sơ mi nam",
        "quantity": 2,
        "unitPrice": 250000,
        "totalPrice": 500000
      }
    ],
    "totalAmount": 500000
  }
]
```

---

## 6. Danh sách API đã hoàn thành

| STT | Phương thức | Endpoint                                     | Chức năng                        |
| --- | ----------- | -------------------------------------------- | -------------------------------- |
| 1   | GET         | `/api/CategoriesProducts`                    | Lấy danh mục sản phẩm            |
| 2   | GET         | `/api/Posts`                                 | Lấy danh sách bài viết           |
| 3   | GET         | `/api/Posts/{id}`                            | Lấy chi tiết bài viết            |
| 4   | GET         | `/api/Posts/category/{categoryId}`           | Lấy bài viết theo danh mục       |
| 5   | GET         | `/api/Products`                              | Lấy danh sách sản phẩm           |
| 6   | GET         | `/api/Products/{id}`                         | Lấy chi tiết sản phẩm            |
| 7   | GET         | `/api/Products/category/{categoryProductId}` | Lấy sản phẩm theo danh mục       |
| 8   | POST        | `/api/Auth/CustomerRegister`                 | Đăng ký khách hàng               |
| 9   | POST        | `/api/Auth/CustomerLogin`                    | Đăng nhập khách hàng             |
| 10  | POST        | `/api/Orders`                                | Tạo đơn hàng                     |
| 11  | GET         | `/api/Orders/customer/{customerId}`          | Lịch sử đơn hàng theo khách hàng |

---

## 7. Kiểm thử API bằng Swagger

Sau khi chạy project, truy cập Swagger UI tại:

```txt
https://localhost:7175/swagger/index.html
```

Các API đã được kiểm thử thành công:

* `GET /api/CategoriesProducts`
* `GET /api/Posts`
* `GET /api/Posts/{id}`
* `GET /api/Products`
* `GET /api/Products/{id}`
* `GET /api/Products/category/{categoryProductId}`
* `POST /api/Auth/CustomerRegister`
* `POST /api/Auth/CustomerLogin`
* `POST /api/Orders`
* `GET /api/Orders/customer/{customerId}`

---

## 8. Kiểm tra dữ liệu trong SQL Server

Một số câu lệnh dùng để kiểm tra dữ liệu:

```sql
SELECT * FROM Customers;
SELECT * FROM Products;
SELECT * FROM Orders;
SELECT * FROM OrderDetails;
```

Kết quả kiểm thử:

* Sau khi đăng ký, dữ liệu khách hàng được thêm vào bảng `Customers`.
* Sau khi đặt hàng, dữ liệu đơn hàng được thêm vào bảng `Orders`.
* Chi tiết sản phẩm trong đơn hàng được thêm vào bảng `OrderDetails`.
* Số lượng tồn kho trong bảng `Products` được khấu trừ sau khi đặt hàng thành công.

---

## 9. Kết quả đạt được

* Hoàn thành nhóm API hiển thị nội dung trang chủ.
* Hoàn thành nhóm API phục vụ trang cửa hàng.
* Hoàn thành API chi tiết sản phẩm.
* Hoàn thành API đăng ký khách hàng.
* Hoàn thành API đăng nhập khách hàng.
* Hoàn thành API xử lý đặt hàng.
* Hoàn thành API lịch sử mua hàng.
* Kiểm thử API thành công bằng Swagger.
* Dữ liệu được lưu và truy xuất từ SQL Server.
* Backend đã sẵn sàng để kết nối với Frontend ReactJS.

---

## 10. Hướng phát triển tiếp theo

Các chức năng có thể phát triển ở những buổi tiếp theo:

* Xây dựng giao diện ReactJS.
* Hiển thị sản phẩm ở `Home.jsx`.
* Hiển thị danh sách sản phẩm ở `Shop.jsx`.
* Hiển thị chi tiết sản phẩm ở `ProductDetail.jsx`.
* Xây dựng giỏ hàng `Cart.jsx`.
* Xây dựng trang thanh toán `Checkout.jsx`.
* Xây dựng trang đăng ký và đăng nhập khách hàng.
* Xây dựng trang lịch sử đơn hàng.
* Nâng cấp bảo mật bằng JWT.
* Mã hóa mật khẩu khách hàng.
* Nâng cấp quản lý trạng thái đơn hàng.
* Bổ sung phân trang, tìm kiếm và lọc sản phẩm.

---

## 11. Kết luận

Ở buổi 06, dự án LOI_CMS đã hoàn thành phần Web API dành cho Frontend ReactJS.

Hệ thống Backend hiện có thể cung cấp dữ liệu danh mục sản phẩm, bài viết, sản phẩm, tài khoản khách hàng và đơn hàng thông qua RESTful API.

Đây là nền tảng để tiếp tục xây dựng giao diện ReactJS và hoàn thiện luồng mua hàng ở các buổi tiếp theo.
