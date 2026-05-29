/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 29/05/2026
 * Mô tả:
 * Controller xử lý chức năng đăng nhập, đăng xuất và phân quyền người dùng.
 * Sử dụng Cookie Authentication để lưu trạng thái đăng nhập.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using CMS.Data;

public class AccountController : Controller
{
    // Biến kết nối tới Database
    private readonly ApplicationDbContext _context;

    // Hàm khởi tạo Controller, nhận ApplicationDbContext thông qua Dependency Injection
    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==================================================
    // HIỂN THỊ FORM ĐĂNG NHẬP
    // ==================================================

    [HttpGet]
    public IActionResult Login()
    {
        // Trả về giao diện Login.cshtml
        return View();
    }

    // ==================================================
    // XỬ LÝ ĐĂNG NHẬP
    // ==================================================

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        // Tìm user trong Database theo username và password
        // Lưu ý: hiện tại password đang so sánh trực tiếp với PasswordHash
        var user = _context.Users
            .FirstOrDefault(u => u.Username == username && u.PasswordHash == password);

        // Nếu tìm thấy tài khoản
        if (user != null)
        {
            // Tạo danh sách Claims để lưu thông tin người dùng vào Cookie
            var claims = new List<Claim>
            {
                // Lưu tên đăng nhập
                new Claim(ClaimTypes.Name, user.Username),

                // Lưu quyền của người dùng, ví dụ: Admin, Editor
                new Claim(ClaimTypes.Role, user.Role),

                // Lưu họ tên đầy đủ để hiển thị trên giao diện Admin
                new Claim("FullName", user.FullName)
            };

            // Tạo danh tính người dùng dựa trên Cookie Authentication
            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            // Đăng nhập người dùng và lưu Cookie vào trình duyệt
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            // Đăng nhập thành công thì chuyển về trang Home
            return RedirectToAction("Index", "Home");
        }

        // Nếu sai tài khoản hoặc mật khẩu thì gửi thông báo lỗi ra View
        ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";

        // Trả lại trang Login
        return View();
    }

    // ==================================================
    // ĐĂNG XUẤT
    // ==================================================

    public async Task<IActionResult> Logout()
    {
        // Xóa Cookie đăng nhập khỏi trình duyệt
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        // Sau khi đăng xuất thì chuyển về trang Login
        return RedirectToAction("Login");
    }

    // ==================================================
    // TRANG KHÔNG CÓ QUYỀN TRUY CẬP
    // ==================================================

    [HttpGet]
    public IActionResult AccessDenied()
    {
        // Trả về giao diện thông báo không có quyền truy cập
        return View();
    }
}