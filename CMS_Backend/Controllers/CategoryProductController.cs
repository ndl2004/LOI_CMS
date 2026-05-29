/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 29/05/2026
 * Mô tả:
 * Controller dùng để quản lý danh mục sản phẩm trong hệ thống CMS.
 * Chức năng:
 * - Hiển thị danh sách danh mục sản phẩm
 * - Thêm danh mục sản phẩm mới
 * - Chỉnh sửa danh mục sản phẩm
 * - Xóa danh mục sản phẩm
 */

using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Backend.Controllers
{
    public class CategoryProductController : Controller
    {
        // Biến kết nối Database
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Hàm khởi tạo Controller
        /// Dependency Injection sẽ tự động truyền DbContext vào
        /// </summary>
        public CategoryProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==================================================
        // HIỂN THỊ DANH SÁCH DANH MỤC SẢN PHẨM
        // ==================================================

        /// <summary>
        /// Hiển thị toàn bộ danh mục sản phẩm
        /// </summary>
        public IActionResult Index()
        {
            var data = _context.CategoriesProducts.ToList();

            return View(data);
        }

        // ==================================================
        // THÊM DANH MỤC SẢN PHẨM
        // ==================================================

        /// <summary>
        /// Hiển thị form thêm danh mục sản phẩm
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Lưu danh mục sản phẩm mới xuống Database
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryProduct model)
        {
            if (ModelState.IsValid)
            {
                _context.CategoriesProducts.Add(model);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ==================================================
        // CHỈNH SỬA DANH MỤC SẢN PHẨM
        // ==================================================

        /// <summary>
        /// Hiển thị form chỉnh sửa
        /// </summary>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var categoryProduct = _context.CategoriesProducts.Find(id);

            if (categoryProduct == null)
            {
                return NotFound();
            }

            return View(categoryProduct);
        }

        /// <summary>
        /// Cập nhật dữ liệu danh mục sản phẩm
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryProduct model)
        {
            if (ModelState.IsValid)
            {
                _context.CategoriesProducts.Update(model);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ==================================================
        // XÓA DANH MỤC SẢN PHẨM
        // ==================================================

        /// <summary>
        /// Hiển thị trang xác nhận xóa
        /// </summary>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var categoryProduct = _context.CategoriesProducts.Find(id);

            if (categoryProduct == null)
            {
                return NotFound();
            }

            return View(categoryProduct);
        }

        /// <summary>
        /// Thực hiện xóa danh mục sản phẩm
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var categoryProduct = _context.CategoriesProducts.Find(id);

            if (categoryProduct != null)
            {
                _context.CategoriesProducts.Remove(categoryProduct);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}