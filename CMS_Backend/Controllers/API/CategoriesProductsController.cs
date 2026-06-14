/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 04/06/2026
 * Mô tả:
 * API Controller dùng để cung cấp dữ liệu danh mục sản phẩm cho Frontend.
 * Chức năng:
 * - Lấy toàn bộ danh sách danh mục sản phẩm
 * - Trả dữ liệu dưới dạng JSON
 * - Hỗ trợ hiển thị menu danh mục sản phẩm ở Frontend ReactJS
 */

using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace CMS_Backend.Controllers.API
{
    /// <summary>
    /// API quản lý danh mục sản phẩm
    /// Đường dẫn mặc định: /api/categoriesproducts
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesProductsController : ControllerBase
    {
        // Biến kết nối Database
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Hàm khởi tạo Controller
        /// Dependency Injection sẽ tự động truyền DbContext vào
        /// </summary>
        /// <param name="context">Đối tượng kết nối Database</param>
        public CategoriesProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==================================================
        // LẤY TOÀN BỘ DANH SÁCH DANH MỤC SẢN PHẨM
        // ==================================================

        /// <summary>
        /// API lấy toàn bộ danh sách danh mục sản phẩm
        /// GET: /api/categoriesproducts
        /// </summary>
        /// <returns>Danh sách danh mục sản phẩm dạng JSON</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _context.CategoriesProducts
                .OrderByDescending(c => c.Id)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    c.ImageUrl
                })
                .ToList();

            return Ok(categories);
        }
    }
}