using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entities;
using CMS_Backend.Services;

namespace CMS_Backend.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public OrdersController(
            ApplicationDbContext context,
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.Id == request.CustomerId);

            if (customer == null)
            {
                return BadRequest(new { message = "Khách hàng không tồn tại" });
            }

            if (request.Items == null || request.Items.Count == 0)
            {
                return BadRequest(new { message = "Giỏ hàng không có sản phẩm" });
            }

            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderDate = DateTime.Now,
                Status = 0,
                Notes = request.Notes
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            decimal totalAmount = 0;
            var productRows = "";

            foreach (var item in request.Items)
            {
                var product = _context.Products
                    .FirstOrDefault(p => p.Id == item.ProductId);

                if (product == null)
                {
                    return BadRequest(new
                    {
                        message = $"Không tìm thấy sản phẩm ID {item.ProductId}"
                    });
                }

                if (product.StockQuantity < item.Quantity)
                {
                    return BadRequest(new
                    {
                        message = $"Sản phẩm {product.Name} không đủ tồn kho"
                    });
                }

                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                _context.OrderDetails.Add(orderDetail);

                product.StockQuantity -= item.Quantity;
                product.SoldQuantity += item.Quantity;

                var itemTotal = product.Price * item.Quantity;
                totalAmount += itemTotal;

                productRows += $@"
                    <tr>
                        <td>{product.Name}</td>
                        <td style='text-align:center'>{item.Quantity}</td>
                        <td style='text-align:right'>{product.Price:N0} đ</td>
                        <td style='text-align:right'>{itemTotal:N0} đ</td>
                    </tr>";
            }

            _context.SaveChanges();

            var receiveEmail = string.IsNullOrWhiteSpace(request.Email)
                ? customer.Email
                : request.Email;

            var emailBody = $@"
                <h2 style='color:#ef3f84'>LOI Cosmetics - Xác nhận đơn hàng</h2>

                <p>Xin chào <strong>{request.FullName ?? customer.FullName}</strong>,</p>
                <p>Cảm ơn bạn đã đặt hàng tại LOI Cosmetics.</p>

                <p><strong>Mã đơn hàng:</strong> #{order.Id}</p>
                <p><strong>Ngày đặt:</strong> {order.OrderDate:dd/MM/yyyy HH:mm}</p>
                <p><strong>Email nhận đơn hàng:</strong> {receiveEmail}</p>
                <p><strong>Họ tên người nhận:</strong> {request.FullName}</p>
                <p><strong>Số điện thoại:</strong> {request.Phone}</p>
                <p><strong>Địa chỉ giao hàng:</strong> {request.Address}</p>

                <table border='1' cellpadding='8' cellspacing='0' style='border-collapse:collapse;width:100%;'>
                    <thead>
                        <tr style='background:#fff0f6'>
                            <th>Sản phẩm</th>
                            <th>Số lượng</th>
                            <th>Đơn giá</th>
                            <th>Thành tiền</th>
                        </tr>
                    </thead>
                    <tbody>
                        {productRows}
                    </tbody>
                </table>

                <h3 style='text-align:right;color:#ef3f84'>
                    Tổng tiền: {totalAmount:N0} đ
                </h3>

                <p>LOI Cosmetics sẽ liên hệ bạn để xác nhận đơn hàng trong thời gian sớm nhất.</p>
            ";

            try
            {
                await _emailService.SendEmailAsync(
                    receiveEmail,
                    "Xác nhận đơn hàng LOI Cosmetics",
                    emailBody
                );
            }
            catch
            {
                // Không làm thất bại đơn hàng nếu gửi email lỗi
            }

            return Ok(new
            {
                message = "Đặt hàng thành công",
                orderId = order.Id,
                customerId = order.CustomerId,
                orderDate = order.OrderDate,
                status = order.Status,
                totalAmount = totalAmount,
                email = receiveEmail
            });
        }

        [HttpGet("customer/{customerId}")]
        public IActionResult GetOrdersByCustomer(int customerId)
        {
            var orders = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    o.Status,
                    o.Notes,
                    Details = o.OrderDetails.Select(d => new
                    {
                        d.Id,
                        d.ProductId,
                        ProductName = d.Product != null ? d.Product.Name : "",
                        d.Quantity,
                        d.UnitPrice,
                        TotalPrice = d.Quantity * d.UnitPrice
                    }),
                    TotalAmount = o.OrderDetails.Sum(d => d.Quantity * d.UnitPrice)
                })
                .ToList();

            return Ok(orders);
        }
    }

    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }

        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public string? Notes { get; set; }

        public List<CreateOrderItemRequest> Items { get; set; }
    }

    public class CreateOrderItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}