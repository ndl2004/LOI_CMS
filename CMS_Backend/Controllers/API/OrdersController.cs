/*
 * Họ tên: Nguyễn Đình Lợi
 * MSSV: 2122110147
 * Lớp: CCQ2211D
 * Ngày tạo: 04/06/2026
 * Mô tả:
 * API Controller dùng để xử lý luồng đặt hàng từ Frontend.
 * Chức năng:
 * - Nhận dữ liệu giỏ hàng từ ReactJS
 * - Tạo đơn hàng mới
 * - Thêm chi tiết đơn hàng vào bảng OrderDetail
 * - Khấu trừ số lượng tồn kho của sản phẩm
 * - Lấy lịch sử đơn hàng theo khách hàng
 * - Trả dữ liệu dưới dạng JSON
 */

using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS_Backend.Controllers.API
{
    /// <summary>
    /// API xử lý đơn hàng
    /// Đường dẫn mặc định: /api/orders
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateOrder(CreateOrderRequest request)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == request.CustomerId);

            if (customer == null)
            {
                return BadRequest(new
                {
                    message = "Khách hàng không tồn tại"
                });
            }

            if (request.Items == null || request.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message = "Giỏ hàng không có sản phẩm"
                });
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

            foreach (var item in request.Items)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);

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

                totalAmount += product.Price * item.Quantity;
            }

            _context.SaveChanges();

            return Ok(new
            {
                message = "Đặt hàng thành công",
                orderId = order.Id,
                customerId = order.CustomerId,
                orderDate = order.OrderDate,
                status = order.Status,
                totalAmount = totalAmount
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
        public string? Notes { get; set; }
        public List<CreateOrderItemRequest> Items { get; set; }
    }

    public class CreateOrderItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}