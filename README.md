# API & BACKEND DEVELOPMENT (BUỔI 06)

Trong buổi 06, project tập trung phát triển phần Backend và Data nhằm chuẩn bị cho việc kết nối ReactJS Frontend với hệ thống CMS thông qua API.

## Nội dung đã thực hiện

* Xây dựng các API Controller bằng ASP.NET Core Web API
* Tạo `PostsController` để cung cấp dữ liệu bài viết dưới dạng JSON
* Sử dụng `[ApiController]` và `[Route("api/[controller]")]`
* Sử dụng `ControllerBase` thay cho `Controller` trong API
* Kết nối API với `ApplicationDbContext`
* Truy vấn dữ liệu bằng Entity Framework Core và LINQ
* Xây dựng API lấy toàn bộ danh sách bài viết:

  * `GET /api/posts`
* Xây dựng API lấy bài viết theo ID:

  * `GET /api/posts/{id}`
* Xây dựng API lấy bài viết theo danh mục:

  * `GET /api/posts/category/{id}`
* Sử dụng `.Where()`, `.Select()`, `.OrderByDescending()` trong LINQ
* Tối ưu dữ liệu trả về bằng cách chỉ lấy các trường cần thiết:

  * Id
  * Title
  * ImageUrl
  * CreatedDate
  * CategoryName
* Trả dữ liệu dưới dạng JSON cho Frontend ReactJS
* Kiểm tra API bằng trình duyệt và Swagger

## Cấu hình Swagger

Trong buổi 06, project đã được bổ sung Swagger để test API trực tiếp.

Đã cấu hình:

* `AddSwaggerGen()`
* `UseSwagger()`
* `UseSwaggerUI()`
* `MapControllers()`

Nhờ đó có thể kiểm tra API tại:

```txt
/swagger
```

## Xử lý dữ liệu và kiểm tra lỗi

* Kiểm tra bài viết tồn tại hay không
* Trả về lỗi `404 Not Found` khi không tìm thấy dữ liệu
* Trả về JSON message thông báo lỗi
* Thực hành test nhiều kịch bản API khác nhau

## Kết quả đạt được

Sau buổi 06, hệ thống đã:

* Có thể hoạt động như một RESTful API Backend
* Trả dữ liệu bài viết dạng JSON
* Kết nối được với Entity Framework Core
* Sẵn sàng cho việc kết nối ReactJS Frontend
* Có Swagger để kiểm tra API nhanh chóng
* Tách rõ Backend API và giao diện Frontend

## Công nghệ sử dụng

* ASP.NET Core Web API
* Entity Framework Core
* LINQ
* Swagger / Swashbuckle
* SQL Server
* JSON API
* Dependency Injection
