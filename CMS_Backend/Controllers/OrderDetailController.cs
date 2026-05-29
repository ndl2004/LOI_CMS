/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 29/05/2026
 * Mô tả:
 * Controller dùng để hiển thị danh sách chi tiết đơn hàng.
 * Chức năng:
 * - Xem danh sách chi tiết đơn hàng
 * - Hiển thị thông tin đơn hàng liên quan
 * - Hiển thị thông tin sản phẩm liên quan
 */

using CMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers
{
    public class OrderDetailController : Controller
    {
        // Biến kết nối Database
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Hàm khởi tạo Controller
        /// Dependency Injection sẽ tự động truyền DbContext vào
        /// </summary>
        /// <param name="context">Đối tượng kết nối Database</param>
        public OrderDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==================================================
        // HIỂN THỊ DANH SÁCH CHI TIẾT ĐƠN HÀNG
        // ==================================================

        /// <summary>
        /// Hiển thị toàn bộ chi tiết đơn hàng
        /// </summary>
        /// <returns>Danh sách chi tiết đơn hàng</returns>
        public IActionResult Index()
        {
            // Lấy dữ liệu OrderDetail kèm thông tin Order và Product
            var details = _context.OrderDetails
                .Include(d => d.Order)
                .Include(d => d.Product)
                .ToList();

            // Trả dữ liệu sang View
            return View(details);
        }
    }
}