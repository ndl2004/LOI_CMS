/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 29/05/2026
 * Mô tả:
 * Controller dùng để quản lý tài khoản người dùng trong hệ thống CMS.
 * Chức năng:
 * - Hiển thị danh sách người dùng
 * - Thêm người dùng mới
 * - Chỉnh sửa thông tin người dùng
 * - Đổi mật khẩu người dùng
 * - Xóa người dùng
 * - Phân quyền truy cập chỉ dành cho Admin
 */

using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Backend.Controllers
{
    /// <summary>
    /// Chỉ tài khoản có Role = Admin mới được phép truy cập
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // Biến kết nối Database
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Hàm khởi tạo Controller
        /// Dependency Injection sẽ tự động truyền DbContext vào
        /// </summary>
        /// <param name="context">Đối tượng kết nối Database</param>
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==================================================
        // HIỂN THỊ DANH SÁCH NGƯỜI DÙNG
        // ==================================================

        /// <summary>
        /// Hiển thị danh sách tất cả người dùng
        /// </summary>
        /// <returns>Danh sách User</returns>
        public IActionResult Index()
        {
            var users = _context.Users.ToList();

            return View(users);
        }

        // ==================================================
        // THÊM NGƯỜI DÙNG
        // ==================================================

        /// <summary>
        /// Hiển thị form thêm người dùng
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Xử lý thêm người dùng mới
        /// </summary>
        /// <param name="model">Thông tin người dùng</param>
        [HttpPost]
        public IActionResult Create(User model)
        {
            // Kiểm tra Username đã tồn tại hay chưa
            var checkExist = _context.Users
                .Any(u => u.Username == model.Username);

            if (checkExist)
            {
                ModelState.AddModelError(
                    "Username",
                    "Tên đăng nhập đã tồn tại!"
                );

                return View(model);
            }

            // Thêm User mới vào Database
            _context.Users.Add(model);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ==================================================
        // CHỈNH SỬA NGƯỜI DÙNG
        // ==================================================

        /// <summary>
        /// Hiển thị form chỉnh sửa người dùng
        /// </summary>
        /// <param name="id">Mã người dùng</param>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        /// <param name="model">Thông tin mới</param>
        /// <param name="NewPassword">Mật khẩu mới</param>
        [HttpPost]
        public IActionResult Edit(User model, string NewPassword)
        {
            // Lấy dữ liệu cũ từ Database
            var existingUser = _context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == model.Id);

            if (existingUser == null)
            {
                return NotFound();
            }

            // Nếu người dùng nhập mật khẩu mới
            if (!string.IsNullOrEmpty(NewPassword))
            {
                model.PasswordHash = NewPassword;
            }
            else
            {
                // Giữ nguyên mật khẩu cũ
                model.PasswordHash = existingUser.PasswordHash;
            }

            // Cập nhật dữ liệu
            _context.Users.Update(model);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ==================================================
        // XÓA NGƯỜI DÙNG
        // ==================================================

        /// <summary>
        /// Hiển thị trang xác nhận xóa
        /// </summary>
        /// <param name="id">Mã người dùng</param>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /// <summary>
        /// Thực hiện xóa người dùng khỏi hệ thống
        /// </summary>
        /// <param name="id">Mã người dùng</param>
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);

            if (user != null)
            {
                _context.Users.Remove(user);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}