# LOI CMS

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-blue?style=for-the-badge)
![C#](https://img.shields.io/badge/C%23-.NET-purple?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL_Server-red?style=for-the-badge)
![Bootstrap](https://img.shields.io/badge/Bootstrap-UI-blueviolet?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Buoi_1_2-success?style=for-the-badge)

## Overview

**LOI CMS** là dự án học tập được xây dựng bằng **ASP.NET Core MVC**, nhằm thực hành phát triển hệ thống quản lý nội dung (CMS) theo từng buổi học.

Hiện tại repository này chứa nội dung đã hoàn thành của **Buổi 1** và **Buổi 2**, tập trung vào việc xây dựng nền tảng project và module quản lý bài viết cơ bản.

---

## Tech Stack

### Backend
- ASP.NET Core MVC
- C#
- Entity Framework Core

### Frontend
- Razor View
- HTML5
- CSS3
- Bootstrap 5

### Database
- SQL Server

### Tools
- Visual Studio 2022
- SQL Server Management Studio
- Git / GitHub

---

## Architecture

```text
Client
  ↓
Controller
  ↓
Model / Entity
  ↓
Entity Framework Core
  ↓
SQL Server
  ↓
View (Razor)
  ↓
Response
```

---

## Current Features

### Buổi 1 — Project Foundation

Đã hoàn thành:

- Khởi tạo project ASP.NET Core MVC
- Cấu hình middleware
- Thiết lập routing
- Static file serving
- MVC folder structure
- Cấu hình môi trường chạy project

---

### Buổi 2 — Post Module

Đã hoàn thành:

- Tạo entity `Post`
- Kết nối database SQL Server
- Hiển thị danh sách bài viết
- Hiển thị chi tiết bài viết
- Render dữ liệu động bằng Razor
- Hiển thị hình ảnh bài viết
- Thiết kế giao diện bằng Bootstrap card layout

---

## Project Structure

```bash
LOI_CMS/
│
├── Controllers/
│   ├── HomeController.cs
│   └── PostController.cs
│
├── Models/
│   └── Entities/
│       └── Post.cs
│
├── Views/
│   ├── Home/
│   └── Post/
│       ├── Index.cshtml
│       └── Details.cshtml
│
├── wwwroot/
│
├── appsettings.json
├── Program.cs
└── README.md
```

---

## Installation

### Clone Repository

```bash
git clone https://github.com/ndl2004/LOI_CMS.git
```

---

### Open Project

Mở bằng:

```bash
Visual Studio 2022
```

---

### Configure Database

Cập nhật connection string trong:

```json
appsettings.json
```

Ví dụ:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=LOI_CMS;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

### Database Migration

Nếu project dùng migrations:

```bash
Update-Database
```

hoặc:

```bash
dotnet ef database update
```

---

### Run Project

```bash
dotnet run
```

hoặc chạy trực tiếp bằng Visual Studio.

---

## Current Status

```text
Completed: Buổi 1 + Buổi 2
Status: In Development
```

---

## Author

**Loi Nguyen**

GitHub:  
https://github.com/ndl2004

---

## License

This project is for learning purposes.
