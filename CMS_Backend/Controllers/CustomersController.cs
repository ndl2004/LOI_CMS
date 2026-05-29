/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 29/05/2026
 * Mô tả:
 * Controller dùng để quản lý khách hàng trong hệ thống CMS.
 * Chức năng:
 * - Hiển thị danh sách khách hàng
 * - Thêm khách hàng mới
 * - Chỉnh sửa thông tin khách hàng
 * - Xóa khách hàng
 */

using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Backend.Controllers
{
    public class CustomerController : Controller
    {
        // Biến kết nối Database
        private readonly ApplicationDbContext _context;

        // Hàm khởi tạo Controller
        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // HIỂN THỊ DANH SÁCH KHÁCH HÀNG
        public IActionResult Index()
        {
            var customers = _context.Customers.ToList();
            return View(customers);
        }

        // HIỂN THỊ FORM THÊM KHÁCH HÀNG
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // XỬ LÝ THÊM KHÁCH HÀNG
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer model)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // HIỂN THỊ FORM SỬA KHÁCH HÀNG
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // XỬ LÝ CẬP NHẬT KHÁCH HÀNG
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer model)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Update(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // HIỂN THỊ TRANG XÁC NHẬN XÓA
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // XỬ LÝ XÓA KHÁCH HÀNG
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}