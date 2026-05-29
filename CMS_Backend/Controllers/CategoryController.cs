/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 17/05/2026
 * Mô tả:
 * Controller dùng để quản lý danh mục bài viết trong hệ thống CMS.
 * Chức năng:
 * - Hiển thị danh sách danh mục
 * - Thêm danh mục mới
 * - Chỉnh sửa danh mục
 * - Xóa danh mục
 * - Bảo vệ chức năng bằng Authorization
 */

using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Chỉ người dùng đã đăng nhập mới được truy cập
/// </summary>
[Authorize]
public class CategoryController : Controller
{
    // Biến kết nối Database
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Hàm khởi tạo Controller
    /// Dependency Injection sẽ tự động truyền DbContext vào
    /// </summary>
    /// <param name="context">Đối tượng kết nối Database</param>
    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==================================================
    // HIỂN THỊ DANH SÁCH DANH MỤC
    // ==================================================

    /// <summary>
    /// Hiển thị toàn bộ danh mục hiện có trong hệ thống
    /// </summary>
    /// <returns>Danh sách danh mục</returns>
    public IActionResult Index()
    {
        // Lấy toàn bộ dữ liệu từ bảng Categories
        var data = _context.Categories.ToList();

        // Trả dữ liệu sang View
        return View(data);
    }

    // ==================================================
    // THÊM DANH MỤC
    // ==================================================

    /// <summary>
    /// Hiển thị form thêm danh mục
    /// </summary>
    /// <returns>View Create</returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Nhận dữ liệu từ form và lưu xuống Database
    /// </summary>
    /// <param name="model">Thông tin danh mục</param>
    /// <returns>Quay về trang danh sách</returns>
    [HttpPost]
    public IActionResult Create(Category model)
    {
        // Thêm đối tượng vào bộ nhớ tạm của Entity Framework
        _context.Categories.Add(model);

        // Lưu dữ liệu thật xuống SQL Server
        _context.SaveChanges();

        // Quay về trang danh sách
        return RedirectToAction("Index");
    }

    // ==================================================
    // XÓA DANH MỤC
    // ==================================================

    /// <summary>
    /// Xóa danh mục theo Id
    /// </summary>
    /// <param name="id">Mã danh mục cần xóa</param>
    /// <returns>Quay về trang danh sách</returns>
    public IActionResult Delete(int id)
    {
        // Tìm danh mục theo Id
        var category = _context.Categories.Find(id);

        // Nếu tìm thấy thì thực hiện xóa
        if (category != null)
        {
            // Đánh dấu đối tượng cần xóa
            _context.Categories.Remove(category);

            // Lưu thay đổi xuống Database
            _context.SaveChanges();
        }

        // Quay về trang danh sách
        return RedirectToAction("Index");
    }

    // ==================================================
    // CHỈNH SỬA DANH MỤC
    // ==================================================

    /// <summary>
    /// Hiển thị form chỉnh sửa danh mục
    /// </summary>
    /// <param name="id">Mã danh mục</param>
    /// <returns>View Edit</returns>
    [HttpGet]
    public IActionResult Edit(int id)
    {
        // Tìm danh mục theo Id
        var category = _context.Categories.Find(id);

        // Nếu không tìm thấy dữ liệu
        if (category == null)
        {
            return NotFound();
        }

        // Trả dữ liệu sang View Edit
        return View(category);
    }

    /// <summary>
    /// Nhận dữ liệu mới và cập nhật xuống Database
    /// </summary>
    /// <param name="model">Thông tin danh mục sau chỉnh sửa</param>
    /// <returns>Quay về trang danh sách</returns>
    [HttpPost]
    public IActionResult Edit(Category model)
    {
        // Cập nhật đối tượng vào Entity Framework
        _context.Categories.Update(model);

        // Lưu thay đổi xuống SQL Server
        _context.SaveChanges();

        // Quay về trang danh sách
        return RedirectToAction("Index");
    }
}