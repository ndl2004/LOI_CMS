/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 29/05/2026
 * Mô tả:
 * HomeController dùng để xử lý các chức năng của trang chủ.
 * Chức năng chính:
 * - Hiển thị các bài viết mới nhất
 * - Kết nối dữ liệu từ Database
 * - Truyền dữ liệu sang View để hiển thị cho người dùng
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;

namespace CMS.Backend.Controllers
{
    /// <summary>
    /// Controller xử lý các chức năng của trang chủ
    /// </summary>
    public class HomeController : Controller
    {
        // Biến kết nối Database
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Hàm khởi tạo Controller
        /// Dependency Injection sẽ tự động truyền DbContext vào
        /// </summary>
        /// <param name="context">Đối tượng kết nối Database</param>
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==================================================
        // TRANG CHỦ
        // ==================================================

        /// <summary>
        /// Hiển thị trang chủ của hệ thống
        /// </summary>
        /// <returns>Danh sách bài viết mới nhất</returns>
        public IActionResult Index()
        {
            /*
             * LINQ Query:
             * Include()           : Lấy thêm dữ liệu Category liên kết với Post
             * OrderByDescending() : Sắp xếp bài viết mới nhất lên đầu
             * Take(3)             : Chỉ lấy 3 bài viết mới nhất
             * ToList()            : Chuyển kết quả truy vấn thành List
             */

            var latestPosts = _context.Posts
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedDate)
                .Take(3)
                .ToList();

            // Truyền danh sách bài viết sang View
            return View(latestPosts);
        }
    }
}