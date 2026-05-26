# Buổi 2 — Module quản lý bài viết

## Mục tiêu

Tiếp tục phát triển project bằng cách xây dựng module quản lý bài viết cơ bản.

---

## Nội dung thực hiện

### 1. Tạo Entity Post

Xây dựng model bài viết:

```csharp
public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
}
```

Thông tin gồm:

- Id
- Title
- Content
- ImageUrl

---

### 2. Kết nối SQL Server

Sử dụng:

- Entity Framework Core
- SQL Server

Cấu hình connection string:

```json
appsettings.json
```

---

### 3. Xây dựng PostController

Tạo controller quản lý bài viết.

Chức năng:

- Lấy danh sách bài viết
- Trả dữ liệu sang view

Ví dụ:

```csharp
Index()
```

---

### 4. Hiển thị danh sách bài viết

Sử dụng:

- Razor View
- foreach
- Model binding

Hiển thị:

- ảnh bài viết
- tiêu đề
- nội dung rút gọn

---

### 5. Thiết kế giao diện

Sử dụng:

```text
Bootstrap 5
```

Layout:

- card design
- responsive layout

---

### 6. Trang chi tiết bài viết

Tạo:

```text
Details action
Details.cshtml
```

Hiển thị:

- ảnh lớn
- tiêu đề
- nội dung đầy đủ

---

## Kết quả đạt được

✅ Entity Post hoàn chỉnh  
✅ SQL Server kết nối thành công  
✅ Danh sách bài viết hoạt động  
✅ Razor render dữ liệu thành công  
✅ Bootstrap UI hoàn chỉnh  
✅ Trang chi tiết bài viết hoạt động  

---

## Kiến thức học được

- Entity Framework Core
- MVC Controller
- Razor syntax
- Model binding
- Bootstrap UI
- CRUD foundation
