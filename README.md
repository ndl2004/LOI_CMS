# Buổi 1 — Khởi tạo dự án ASP.NET Core MVC

## Mục tiêu

Buổi học đầu tiên tập trung vào việc khởi tạo project và làm quen với kiến trúc ASP.NET Core MVC.

---

## Nội dung thực hiện

### 1. Khởi tạo project

Tạo project mới bằng:

```text
ASP.NET Core Web App (Model-View-Controller)
```

Môi trường sử dụng:

- Visual Studio 2022
- .NET
- ASP.NET Core MVC

---

### 2. Thiết lập cấu trúc MVC

Làm quen với cấu trúc project:

```text
Controllers/
Models/
Views/
wwwroot/
```

Ý nghĩa:

- Controllers → xử lý request
- Models → dữ liệu / business model
- Views → giao diện
- wwwroot → css / js / images

---

### 3. Cấu hình Program.cs

Thiết lập middleware:

```csharp
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
```

Thiết lập routing:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

---

### 4. Hiểu luồng MVC

Luồng xử lý request:

```text
Browser
 ↓
Controller
 ↓
Model
 ↓
View
 ↓
Response
```

---

## Kết quả đạt được

✅ Project chạy thành công  
✅ MVC hoạt động đúng  
✅ Routing hoạt động  
✅ Static files load thành công  
✅ Hiểu cấu trúc project ASP.NET Core MVC  

---

## Kiến thức học được

- MVC Pattern
- Request pipeline
- Middleware
- Routing
- Project structure
