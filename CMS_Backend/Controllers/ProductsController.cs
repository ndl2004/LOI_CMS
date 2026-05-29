/*Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 29/05/2026
 * Mô tả:
 * Controller dùng để quản lý sản phẩm trong hệ thống CMS.
 * Chức năng:
 * - Hiển thị danh sách sản phẩm
 * - Thêm sản phẩm mới
 * - Upload ảnh sản phẩm
 * - Chỉnh sửa sản phẩm
 * - Cập nhật ảnh sản phẩm
 * - Xóa sản phẩm
 */

using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers
{
    /// <summary>
    /// Controller quản lý sản phẩm
    /// </summary>
    public class ProductController : Controller
    {
        // Biến kết nối Database
        private readonly ApplicationDbContext _context;

        // Biến dùng để truy cập thư mục wwwroot
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Hàm khởi tạo Controller
        /// Dependency Injection sẽ tự động truyền DbContext
        /// và IWebHostEnvironment vào hệ thống
        /// </summary>
        public ProductController(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ==================================================
        // HIỂN THỊ DANH SÁCH SẢN PHẨM
        // ==================================================

        /// <summary>
        /// Hiển thị toàn bộ sản phẩm trong hệ thống
        /// </summary>
        /// <returns>Danh sách sản phẩm</returns>
        public IActionResult Index()
        {
            // Lấy danh sách sản phẩm và danh mục liên quan
            var products = _context.Products
                .Include(p => p.CategoryProduct)
                .ToList();

            // Trả dữ liệu sang View
            return View(products);
        }

        // ==================================================
        // THÊM SẢN PHẨM
        // ==================================================

        /// <summary>
        /// Hiển thị form thêm sản phẩm
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Xử lý thêm sản phẩm mới
        /// </summary>
        /// <param name="model">Thông tin sản phẩm</param>
        /// <param name="ImageFile">Ảnh sản phẩm upload từ máy tính</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model, IFormFile? ImageFile)
        {
            // Nếu người dùng chọn ảnh
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Đường dẫn thư mục lưu ảnh
                string uploadFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "products");

                // Nếu chưa tồn tại thì tạo thư mục
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Tạo tên file ngẫu nhiên tránh trùng tên
                string fileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(ImageFile.FileName);

                // Tạo đường dẫn vật lý
                string filePath =
                    Path.Combine(uploadFolder, fileName);

                // Lưu file vào server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                // Lưu đường dẫn ảnh vào Database
                model.ImageUrl =
                    "/uploads/products/" + fileName;
            }

            // Kiểm tra dữ liệu hợp lệ
            if (ModelState.IsValid)
            {
                _context.Products.Add(model);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ==================================================
        // CHỈNH SỬA SẢN PHẨM
        // ==================================================

        /// <summary>
        /// Hiển thị form chỉnh sửa sản phẩm
        /// </summary>
        /// <param name="id">Mã sản phẩm</param>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Tìm sản phẩm theo ID
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        /// <summary>
        /// Cập nhật sản phẩm
        /// </summary>
        /// <param name="model">Dữ liệu sản phẩm</param>
        /// <param name="ImageFile">Ảnh mới upload</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model, IFormFile? ImageFile)
        {
            // Lấy dữ liệu cũ
            var existingProduct = _context.Products
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == model.Id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            // Nếu chọn ảnh mới
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "products");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string fileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(ImageFile.FileName);

                string filePath =
                    Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                model.ImageUrl =
                    "/uploads/products/" + fileName;
            }
            else
            {
                // Giữ nguyên ảnh cũ nếu không upload ảnh mới
                model.ImageUrl = existingProduct.ImageUrl;
            }

            if (ModelState.IsValid)
            {
                _context.Products.Update(model);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ==================================================
        // XÓA SẢN PHẨM
        // ==================================================

        /// <summary>
        /// Hiển thị trang xác nhận xóa
        /// </summary>
        /// <param name="id">Mã sản phẩm</param>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _context.Products
                .Include(p => p.CategoryProduct)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        /// <summary>
        /// Thực hiện xóa sản phẩm khỏi Database
        /// </summary>
        /// <param name="id">Mã sản phẩm</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Tìm sản phẩm cần xóa
            var product = _context.Products.Find(id);

            if (product != null)
            {
                _context.Products.Remove(product);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}