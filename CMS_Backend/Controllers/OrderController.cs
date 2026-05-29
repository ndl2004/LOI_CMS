/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 29/05/2026
 * Mô tả:
 * Controller dùng để quản lý đơn hàng trong hệ thống CMS.
 * Chức năng:
 * - Hiển thị danh sách đơn hàng
 * - Thêm đơn hàng mới
 * - Chỉnh sửa đơn hàng
 * - Xóa đơn hàng
 * - Liên kết đơn hàng với khách hàng
 */

using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // HIỂN THỊ DANH SÁCH ĐƠN HÀNG
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .ToList();

            return View(orders);
        }

        // HIỂN THỊ FORM THÊM ĐƠN HÀNG
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Customers = new SelectList(_context.Customers, "Id", "FullName");
            return View();
        }

        // XỬ LÝ THÊM ĐƠN HÀNG
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order model)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Customers = new SelectList(_context.Customers, "Id", "FullName", model.CustomerId);
            return View(model);
        }

        // HIỂN THỊ FORM SỬA ĐƠN HÀNG
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Customers = new SelectList(_context.Customers, "Id", "FullName", order.CustomerId);
            return View(order);
        }

        // XỬ LÝ CẬP NHẬT ĐƠN HÀNG
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order model)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Update(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Customers = new SelectList(_context.Customers, "Id", "FullName", model.CustomerId);
            return View(model);
        }

        // HIỂN THỊ TRANG XÁC NHẬN XÓA
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // XỬ LÝ XÓA ĐƠN HÀNG
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _context.Orders.Find(id);

            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}