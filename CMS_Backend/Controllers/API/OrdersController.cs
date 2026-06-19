using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entities;
using CMS_Backend.Services;
using Microsoft.EntityFrameworkCore;

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

            using var transaction = _context.Database.BeginTransaction();

            var receiveEmail = string.IsNullOrWhiteSpace(request.Email)
                ? customer.Email
                : request.Email;

            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderDate = DateTime.Now,
                Status = 0,
                ReceiverName = string.IsNullOrWhiteSpace(request.FullName)
                    ? customer.FullName
                    : request.FullName.Trim(),
                ReceiverEmail = receiveEmail,
                ReceiverPhone = string.IsNullOrWhiteSpace(request.Phone)
                    ? customer.Phone
                    : request.Phone.Trim(),
                ShippingAddress = string.IsNullOrWhiteSpace(request.Address)
                    ? customer.Address
                    : request.Address.Trim(),
                ShippingFee = 0,
                PaymentMethod = "COD",
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

                var now = DateTime.Now;
                var flashSaleItem = _context.FlashSaleItems
                    .Include(x => x.FlashSale)
                    .FirstOrDefault(x =>
                        x.ProductId == product.Id &&
                        x.FlashSale != null &&
                        x.FlashSale.IsActive &&
                        x.FlashSale.StartTime <= now &&
                        x.FlashSale.EndTime >= now);

                if (flashSaleItem != null &&
                    flashSaleItem.SaleQuantity > 0 &&
                    flashSaleItem.SoldQuantity + item.Quantity > flashSaleItem.SaleQuantity)
                {
                    return BadRequest(new
                    {
                        message = $"Sản phẩm {product.Name} không đủ số lượng khuyến mãi"
                    });
                }

                var unitPrice = product.Price;
                var isFlashSale = false;
                var discountPercent = 0;

                if (flashSaleItem != null)
                {
                    unitPrice = product.Price - (product.Price * flashSaleItem.DiscountPercent / 100);
                    isFlashSale = true;
                    discountPercent = flashSaleItem.DiscountPercent;
                    flashSaleItem.SoldQuantity += item.Quantity;
                }

                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    OriginalPrice = product.Price,
                    DiscountPercent = discountPercent,
                    IsFlashSale = isFlashSale
                };

                _context.OrderDetails.Add(orderDetail);

                product.StockQuantity -= item.Quantity;
                product.SoldQuantity += item.Quantity;

                var itemTotal = unitPrice * item.Quantity;
                totalAmount += itemTotal;

                productRows += $@"
                    <tr>
                        <td>{product.Name}</td>
                        <td style='text-align:center'>{item.Quantity}</td>
                        <td style='text-align:right'>{unitPrice:N0} đ</td>
                        <td style='text-align:right'>{itemTotal:N0} đ</td>
                    </tr>";
            }

            order.TotalAmount = totalAmount + order.ShippingFee;

            _context.SaveChanges();
            transaction.Commit();

            var emailBody = $@"
                <h2 style='color:#ef3f84'>LOI Cosmetics - Xác nhận đơn hàng</h2>

                <p>Xin chào <strong>{order.ReceiverName}</strong>,</p>
                <p>Cảm ơn bạn đã đặt hàng tại LOI Cosmetics.</p>

                <p><strong>Mã đơn hàng:</strong> #{order.Id}</p>
                <p><strong>Ngày đặt:</strong> {order.OrderDate:dd/MM/yyyy HH:mm}</p>
                <p><strong>Email nhận đơn hàng:</strong> {order.ReceiverEmail}</p>
                <p><strong>Họ tên người nhận:</strong> {order.ReceiverName}</p>
                <p><strong>Số điện thoại:</strong> {order.ReceiverPhone}</p>
                <p><strong>Địa chỉ giao hàng:</strong> {order.ShippingAddress}</p>

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
                    Tổng tiền: {order.TotalAmount:N0} đ
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
                totalAmount = order.TotalAmount,
                email = order.ReceiverEmail
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
                    o.ReceiverName,
                    o.ReceiverEmail,
                    o.ReceiverPhone,
                    o.ShippingAddress,
                    o.ShippingFee,
                    o.PaymentMethod,
                    Details = o.OrderDetails.Select(d => new
                    {
                        d.Id,
                        d.ProductId,
                        ProductName = !string.IsNullOrWhiteSpace(d.ProductName)
                            ? d.ProductName
                            : d.Product != null ? d.Product.Name : "",
                        d.Quantity,
                        d.UnitPrice,
                        OriginalPrice = d.OriginalPrice > 0
                            ? d.OriginalPrice
                            : d.Product != null ? d.Product.Price : d.UnitPrice,
                        d.DiscountPercent,
                        IsFlashSale = d.IsFlashSale ||
                            ((d.OriginalPrice > 0
                                ? d.OriginalPrice
                                : d.Product != null ? d.Product.Price : d.UnitPrice) > d.UnitPrice),
                        TotalPrice = d.Quantity * d.UnitPrice
                    }),
                    TotalAmount = o.TotalAmount > 0
                        ? o.TotalAmount
                        : o.OrderDetails.Sum(d => d.Quantity * d.UnitPrice)
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
