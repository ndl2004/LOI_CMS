using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;

namespace CMS.Backend.Controllers
{
    public class HomeController : Controller
    {
        // Khai báo biến DbContext để kết nối database
        private readonly ApplicationDbContext _context;

        // Constructor: nhận DbContext từ Dependency Injection
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trang chủ
        public IActionResult Index()
        {
            // LINQ Query:
            // Include(): lấy thêm dữ liệu từ bảng Category liên kết với Post
            // OrderByDescending(): sắp xếp bài viết mới nhất lên đầu
            // Take(3): chỉ lấy 3 bài viết mới nhất
            // ToList(): chuyển kết quả query thành danh sách
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