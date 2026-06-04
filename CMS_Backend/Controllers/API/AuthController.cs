/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 04/06/2026
 * Mô tả:
 * API Controller dùng để xử lý đăng ký và đăng nhập tài khoản khách hàng.
 * Chức năng:
 * - Đăng ký tài khoản khách hàng
 * - Đăng nhập tài khoản khách hàng
 * - Trả dữ liệu khách hàng dưới dạng JSON
 * - Hỗ trợ Frontend ReactJS nhận diện người mua hàng
 */

using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS_Backend.Controllers.API
{
    /// <summary>
    /// API xác thực tài khoản khách hàng
    /// Đường dẫn mặc định: /api/auth
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("CustomerRegister")]
        public IActionResult CustomerRegister(CustomerRegisterRequest request)
        {
            var exists = _context.Customers.Any(c => c.Email == request.Email);

            if (exists)
            {
                return BadRequest(new
                {
                    message = "Email này đã được đăng ký"
                });
            }

            var customer = new Customer
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = request.Password,
                Phone = request.Phone,
                Address = request.Address
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Đăng ký tài khoản thành công",
                customer.Id,
                customer.FullName,
                customer.Email,
                customer.Phone,
                customer.Address
            });
        }

        [HttpPost("CustomerLogin")]
        public IActionResult CustomerLogin(CustomerLoginRequest request)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.Email == request.Email && c.Password == request.Password);

            if (customer == null)
            {
                return Unauthorized(new
                {
                    message = "Email hoặc mật khẩu không đúng"
                });
            }

            return Ok(new
            {
                message = "Đăng nhập thành công",
                customer.Id,
                customer.FullName,
                customer.Email,
                customer.Phone,
                customer.Address
            });
        }
    }

    public class CustomerRegisterRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class CustomerLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}