/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 17/05/2026
 * Mô tả:
 * Controller dùng để quản lý bài viết trong hệ thống CMS.
 * Chức năng:
 * - Hiển thị danh sách bài viết
 * - Lọc bài viết theo danh mục
 * - Xem chi tiết bài viết
 * - Thêm bài viết mới
 * - Upload hình ảnh bài viết
 * - Chỉnh sửa bài viết
 * - Cập nhật hình ảnh bài viết
 * - Xóa bài viết
 * - Yêu cầu người dùng đăng nhập trước khi truy cập
 */

using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Backend.Controllers
{
    /// <summary>
    /// Chỉ người dùng đã đăng nhập mới được truy cập
    /// </summary>
    [Authorize]
    public class PostController : Controller
    {
        // Biến kết nối Database
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Hàm khởi tạo Controller
        /// Dependency Injection sẽ tự động truyền DbContext vào
        /// </summary>
        /// <param name="context">Đối tượng kết nối Database</param>
        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==================================================
        // HIỂN THỊ DANH SÁCH BÀI VIẾT
        // ==================================================

        /// <summary>
        /// Hiển thị danh sách bài viết
        /// Có thể lọc theo CategoryId
        /// </summary>
        /// <param name="id">Mã danh mục (không bắt buộc)</param>
        /// <returns>Danh sách bài viết</returns>
        public IActionResult Index(int? id)
        {
            // Lấy danh sách bài viết và danh mục liên quan
            var posts = _context.Posts
                .Include(p => p.Category)
                .AsQueryable();

            // Nếu có CategoryId thì lọc theo danh mục
            if (id != null)
            {
                posts = posts.Where(p => p.CategoryId == id);
            }

            // Sắp xếp bài viết mới nhất lên đầu
            var result = posts
                .OrderByDescending(p => p.CreatedDate)
                .ToList();

            return View(result);
        }

        // ==================================================
        // XEM CHI TIẾT BÀI VIẾT
        // ==================================================

        /// <summary>
        /// Hiển thị chi tiết bài viết
        /// </summary>
        /// <param name="id">Mã bài viết</param>
        /// <returns>Thông tin chi tiết bài viết</returns>
        public IActionResult Details(int id)
        {
            var post = _context.Posts
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // ==================================================
        // THÊM BÀI VIẾT
        // ==================================================

        /// <summary>
        /// Hiển thị form thêm bài viết
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            // Tạo danh sách danh mục cho dropdown
            ViewBag.CategoryList = new SelectList(
                _context.Categories,
                "Id",
                "Name"
            );

            return View();
        }

        /// <summary>
        /// Xử lý thêm bài viết mới
        /// </summary>
        /// <param name="model">Thông tin bài viết</param>
        /// <param name="uploadImage">Ảnh upload từ máy tính</param>
        [HttpPost]
        public IActionResult Create(Post model, IFormFile uploadImage)
        {
            // Nếu có upload ảnh
            if (uploadImage != null && uploadImage.Length > 0)
            {
                string folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads"
                );

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(uploadImage.FileName);

                string filePath =
                    Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadImage.CopyTo(stream);
                }

                model.ImageUrl = "/uploads/" + fileName;
            }

            _context.Posts.Add(model);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ==================================================
        // XÓA BÀI VIẾT
        // ==================================================

        /// <summary>
        /// Xóa bài viết theo Id
        /// </summary>
        /// <param name="id">Mã bài viết</param>
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Find(id);

            if (post != null)
            {
                _context.Posts.Remove(post);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ==================================================
        // CHỈNH SỬA BÀI VIẾT
        // ==================================================

        /// <summary>
        /// Hiển thị form chỉnh sửa bài viết
        /// </summary>
        /// <param name="id">Mã bài viết</param>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Find(id);

            if (post == null)
            {
                return NotFound();
            }

            ViewBag.CategoryList = new SelectList(
                _context.Categories,
                "Id",
                "Name",
                post.CategoryId
            );

            return View(post);
        }

        /// <summary>
        /// Cập nhật thông tin bài viết
        /// </summary>
        /// <param name="model">Dữ liệu bài viết mới</param>
        /// <param name="uploadImage">Ảnh mới upload</param>
        [HttpPost]
        public IActionResult Edit(Post model, IFormFile uploadImage)
        {
            // Nếu người dùng upload ảnh mới
            if (uploadImage != null && uploadImage.Length > 0)
            {
                string folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads"
                );

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(uploadImage.FileName);

                string filePath =
                    Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadImage.CopyTo(stream);
                }

                model.ImageUrl = "/uploads/" + fileName;
            }
            else
            {
                // Giữ nguyên ảnh cũ nếu không chọn ảnh mới
                var oldPost = _context.Posts
                    .AsNoTracking()
                    .FirstOrDefault(p => p.Id == model.Id);

                if (oldPost != null &&
                    string.IsNullOrEmpty(model.ImageUrl))
                {
                    model.ImageUrl = oldPost.ImageUrl;
                }
            }

            _context.Posts.Update(model);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}