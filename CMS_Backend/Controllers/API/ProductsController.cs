/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 04/06/2026
 * Mô tả:
 * API Controller dùng để cung cấp dữ liệu sản phẩm cho Frontend.
 * Chức năng:
 * - Lấy toàn bộ danh sách sản phẩm
 * - Lấy danh sách sản phẩm theo danh mục
 * - Lấy chi tiết sản phẩm theo ID
 * - Trả dữ liệu dưới dạng JSON
 * - Hỗ trợ kết nối Frontend ReactJS thông qua RESTful API
 */

using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace CMS_Backend.Controllers.API
{
    /// <summary>
    /// API quản lý sản phẩm
    /// Đường dẫn mặc định: /api/products
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Biến kết nối Database
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Hàm khởi tạo Controller
        /// Dependency Injection sẽ tự động truyền DbContext vào
        /// </summary>
        /// <param name="context">Đối tượng kết nối Database</param>
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==================================================
        // LẤY TOÀN BỘ DANH SÁCH SẢN PHẨM
        // ==================================================

        /// <summary>
        /// API lấy toàn bộ danh sách sản phẩm
        /// GET: /api/products
        /// </summary>
        /// <returns>Danh sách sản phẩm dạng JSON</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _context.Products
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.ImageUrl,
                    p.CategoryProductId
                })
                .ToList();

            return Ok(products);
        }

        // ==================================================
        // LẤY DANH SÁCH SẢN PHẨM THEO DANH MỤC
        // ==================================================

        /// <summary>
        /// API lấy sản phẩm theo CategoryProductId
        /// GET: /api/products/category/{categoryProductId}
        /// </summary>
        /// <param name="categoryProductId">Mã danh mục sản phẩm</param>
        /// <returns>Danh sách sản phẩm thuộc danh mục</returns>
        [HttpGet("category/{categoryProductId}")]
        public IActionResult GetByCategory(int categoryProductId)
        {
            var products = _context.Products
                .Where(p => p.CategoryProductId == categoryProductId)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.ImageUrl
                })
                .ToList();

            return Ok(products);
        }

        // ==================================================
        // LẤY CHI TIẾT SẢN PHẨM
        // ==================================================

        /// <summary>
        /// API lấy chi tiết sản phẩm theo ID
        /// GET: /api/products/{id}
        /// </summary>
        /// <param name="id">Mã sản phẩm</param>
        /// <returns>Thông tin chi tiết sản phẩm</returns>
        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var product = _context.Products
                .Include(p => p.CategoryProduct)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.ImageUrl,
                    p.CategoryProductId,

                    CategoryName = p.CategoryProduct != null
                        ? p.CategoryProduct.Name
                        : "Chưa có danh mục"
                })
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy sản phẩm trong hệ thống"
                });
            }

            return Ok(product);
        }
    }
}