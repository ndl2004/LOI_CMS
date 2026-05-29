/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 29/05/2026
 * Mô tả:
 * API Controller dùng để cung cấp dữ liệu bài viết cho Frontend.
 * Chức năng:
 * - Lấy toàn bộ danh sách bài viết
 * - Lấy danh sách bài viết theo danh mục
 * - Lấy chi tiết bài viết theo ID
 * - Trả dữ liệu dưới dạng JSON
 * - Hỗ trợ kết nối Frontend thông qua RESTful API
 */

using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers
{
    /// <summary>
    /// API quản lý bài viết
    /// Đường dẫn mặc định: /api/posts
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        // Biến kết nối Database
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Hàm khởi tạo Controller
        /// Dependency Injection sẽ tự động truyền DbContext vào
        /// </summary>
        /// <param name="context">Đối tượng kết nối Database</param>
        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==================================================
        // LẤY TOÀN BỘ DANH SÁCH BÀI VIẾT
        // ==================================================

        /// <summary>
        /// API lấy toàn bộ danh sách bài viết
        /// GET: /api/posts
        /// </summary>
        /// <returns>Danh sách bài viết dạng JSON</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            // Lấy dữ liệu từ bảng Posts
            // Include() dùng để lấy thêm dữ liệu Category liên quan
            var posts = _context.Posts
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.ImageUrl,
                    p.CreatedDate,

                    // Hiển thị tên danh mục
                    CategoryName = p.Category != null
                        ? p.Category.Name
                        : "Chưa có danh mục"
                })
                .ToList();

            // Trả dữ liệu kèm mã trạng thái HTTP 200
            return Ok(posts);
        }

        // ==================================================
        // LẤY DANH SÁCH BÀI VIẾT THEO DANH MỤC
        // ==================================================

        /// <summary>
        /// API lấy bài viết theo CategoryId
        /// GET: /api/posts/category/{categoryId}
        /// </summary>
        /// <param name="categoryId">Mã danh mục</param>
        /// <returns>Danh sách bài viết thuộc danh mục</returns>
        [HttpGet("category/{categoryId}")]
        public IActionResult GetByCategory(int categoryId)
        {
            // Lọc bài viết theo CategoryId
            var posts = _context.Posts
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.ImageUrl,
                    p.CreatedDate
                })
                .ToList();

            // Trả dữ liệu JSON
            return Ok(posts);
        }

        // ==================================================
        // LẤY CHI TIẾT BÀI VIẾT
        // ==================================================

        /// <summary>
        /// API lấy chi tiết bài viết theo ID
        /// GET: /api/posts/{id}
        /// </summary>
        /// <param name="id">Mã bài viết</param>
        /// <returns>Thông tin chi tiết bài viết</returns>
        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            // Tìm bài viết theo ID
            var post = _context.Posts
                .FirstOrDefault(p => p.Id == id);

            // Nếu không tìm thấy dữ liệu
            if (post == null)
            {
                // Trả về lỗi HTTP 404
                return NotFound(new
                {
                    message = "Không tìm thấy bài viết này trong hệ thống"
                });
            }

            // Trả về dữ liệu kèm HTTP 200
            return Ok(post);
        }
    }
}