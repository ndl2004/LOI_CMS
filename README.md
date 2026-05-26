# Buổi 3 — Hoàn thiện CRUD bài viết (Create / Edit / Delete)

## Mục tiêu

Trong buổi học này, dự án tiếp tục phát triển module quản lý bài viết bằng cách hoàn thiện các chức năng CRUD còn thiếu.

Mục tiêu chính:

- Tạo bài viết mới
- Chỉnh sửa bài viết
- Xóa bài viết
- Hoàn thiện quy trình quản lý nội dung cơ bản

---

## Nội dung thực hiện

### 1. Chức năng tạo bài viết (Create Post)

Xây dựng action tạo bài viết mới.

Bao gồm:

- GET Create → hiển thị form nhập dữ liệu
- POST Create → nhận dữ liệu từ form và lưu vào database

Thông tin nhập:

- Tiêu đề bài viết
- Nội dung bài viết
- Hình ảnh

Ví dụ:

```csharp
Create()
```

---

### 2. Form nhập dữ liệu

Tạo giao diện nhập bài viết bằng Razor View.

Thành phần:

- Input tiêu đề
- Textarea nội dung
- Input ảnh
- Nút submit

Sử dụng:

- Razor syntax
- Bootstrap form UI

---

### 3. Chỉnh sửa bài viết (Edit Post)

Xây dựng chức năng cập nhật bài viết.

Bao gồm:

- GET Edit → load dữ liệu hiện tại
- POST Edit → cập nhật dữ liệu mới

Chức năng:

- sửa tiêu đề
- sửa nội dung
- sửa ảnh

---

### 4. Xóa bài viết (Delete Post)

Xây dựng chức năng xóa bài viết.

Quy trình:

- chọn bài viết
- xác nhận xóa
- xóa khỏi database

---

### 5. Hoàn thiện CRUD

Sau buổi này module Post đã có:

- Create
- Read
- Update
- Delete

---

## Kết quả đạt được

✅ Tạo bài viết mới thành công  
✅ Chỉnh sửa bài viết thành công  
✅ Xóa bài viết thành công  
✅ CRUD hoàn chỉnh  
✅ Kết nối database ổn định  
✅ Bootstrap UI cho form quản lý  

---

## Kiến thức học được

- CRUD operations
- HTTP GET / POST
- Form handling trong ASP.NET Core MVC
- Model binding
- Entity Framework update
- Delete workflow
- Razor form development
