using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entities;
using CMS_Backend.Services;

namespace CMS_Backend.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AuthController(
            ApplicationDbContext context,
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
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
                .FirstOrDefault(c => c.Email == request.Email);

            if (customer == null)
            {
                return Unauthorized(new
                {
                    message = "Email hoặc mật khẩu không đúng"
                });
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(
                request.Password,
                customer.Password
            );

            if (!isValidPassword)
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

        [HttpPost("SendForgotPasswordOtp")]
        public async Task<IActionResult> SendForgotPasswordOtp(
            ForgotPasswordOtpRequest request)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.Email == request.Email);

            if (customer == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy tài khoản với email này"
                });
            }

            var otp = new Random().Next(100000, 999999).ToString();

            customer.ResetPasswordOtp = otp;
            customer.ResetPasswordOtpExpiry = DateTime.Now.AddMinutes(5);

            _context.SaveChanges();

            var emailBody = $@"
                <h2 style='color:#ef3f84'>
                    LOI Cosmetics - Mã xác nhận đặt lại mật khẩu
                </h2>

                <p>Xin chào <strong>{customer.FullName}</strong>,</p>

                <p>
                    Bạn vừa yêu cầu đặt lại mật khẩu cho tài khoản LOI Cosmetics.
                </p>

                <p>Mã OTP của bạn là:</p>

                <h1 style='letter-spacing:4px;color:#ef3f84'>
                    {otp}
                </h1>

                <p>
                    Mã này có hiệu lực trong <strong>5 phút</strong>.
                </p>

                <p>
                    Nếu bạn không yêu cầu thao tác này, vui lòng bỏ qua email này.
                </p>
            ";

            await _emailService.SendEmailAsync(
                customer.Email,
                "Mã OTP đặt lại mật khẩu LOI Cosmetics",
                emailBody
            );

            return Ok(new
            {
                message = "Mã OTP đã được gửi đến email của bạn"
            });
        }

        [HttpPost("ResetPasswordWithOtp")]
        public IActionResult ResetPasswordWithOtp(
            ResetPasswordWithOtpRequest request)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.Email == request.Email);

            if (customer == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy tài khoản"
                });
            }

            if (customer.ResetPasswordOtp != request.Otp)
            {
                return BadRequest(new
                {
                    message = "Mã OTP không chính xác"
                });
            }

            if (customer.ResetPasswordOtpExpiry == null ||
                customer.ResetPasswordOtpExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    message = "Mã OTP đã hết hạn"
                });
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword) ||
                request.NewPassword.Length < 6)
            {
                return BadRequest(new
                {
                    message = "Mật khẩu mới phải từ 6 ký tự trở lên"
                });
            }

            customer.Password = BCrypt.Net.BCrypt.HashPassword(
                request.NewPassword
            );

            customer.ResetPasswordOtp = null;
            customer.ResetPasswordOtpExpiry = null;

            _context.SaveChanges();

            return Ok(new
            {
                message = "Đặt lại mật khẩu thành công"
            });
        }

        [HttpPost("UpdateProfile")]
        public IActionResult UpdateProfile(UpdateProfileRequest request)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.Id == request.Id);

            if (customer == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy khách hàng"
                });
            }

            var emailExists = _context.Customers
                .Any(c => c.Email == request.Email && c.Id != request.Id);

            if (emailExists)
            {
                return BadRequest(new
                {
                    message = "Email này đã được sử dụng bởi tài khoản khác"
                });
            }

            customer.FullName = request.FullName;
            customer.Email = request.Email;
            customer.Phone = request.Phone;
            customer.Address = request.Address;

            _context.SaveChanges();

            return Ok(new
            {
                message = "Cập nhật hồ sơ thành công",
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

    public class ForgotPasswordOtpRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordWithOtpRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
    }

    public class UpdateProfileRequest
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}