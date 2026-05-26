# Buổi 4 — Quản lý danh mục (Category Management)

## Mục tiêu

Buổi học này mở rộng hệ thống CMS bằng cách xây dựng module quản lý danh mục bài viết.

Mục tiêu:

- Tạo module Category
- Quản lý danh mục
- Kết nối bài viết với danh mục
- Mở rộng cấu trúc CMS thực tế

---

## Nội dung thực hiện

### 1. Tạo Entity Category

Xây dựng model danh mục.

Thông tin:

- Id
- Name
- Description

Ví dụ:

```csharp
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
```

---

### 2. Kết nối database

Cập nhật database để thêm bảng category.

Bao gồm:

- migration
- update database
- cấu hình Entity Framework

---

### 3. Xây dựng CategoryController

Tạo controller quản lý danh mục.

Chức năng:

- danh sách category
- tạo category
- chỉnh sửa category
- xóa category

---

### 4. Giao diện quản lý category

Thiết kế giao diện quản trị.

Bao gồm:

- bảng danh sách
- form thêm mới
- form chỉnh sửa
- nút thao tác

Sử dụng:

- Bootstrap 5
- Razor View

---

### 5. Mở rộng CMS architecture

Hệ thống bắt đầu có nhiều module:

```text
Post Management
Category Management
```

CMS trở nên gần với hệ thống thực tế hơn.

---

## Kết quả đạt được

✅ Module Category hoàn chỉnh  
✅ CRUD danh mục hoạt động  
✅ Database mở rộng thành công  
✅ UI quản lý danh mục hoàn chỉnh  
✅ CMS structure rõ ràng hơn  

---

## Kiến thức học được

- Entity relationship design
- Multi-module MVC architecture
- Database schema expansion
- CRUD module architecture
- Controller organization
