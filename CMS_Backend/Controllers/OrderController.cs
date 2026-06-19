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
using CMS_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public OrderController(
            ApplicationDbContext context,
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // HIỂN THỊ DANH SÁCH ĐƠN HÀNG
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
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
            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.ReceiverName ??= order.Customer?.FullName;
            order.ReceiverEmail ??= order.Customer?.Email;
            order.ReceiverPhone ??= order.Customer?.Phone;
            order.ShippingAddress ??= order.Customer?.Address;
            order.PaymentMethod ??= "COD";

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
                var order = _context.Orders.Find(model.Id);

                if (order == null)
                {
                    return NotFound();
                }

                order.CustomerId = model.CustomerId;
                order.OrderDate = model.OrderDate;
                order.Status = model.Status;
                order.ReceiverName = model.ReceiverName;
                order.ReceiverEmail = model.ReceiverEmail;
                order.ReceiverPhone = model.ReceiverPhone;
                order.ShippingAddress = model.ShippingAddress;
                order.ShippingFee = model.ShippingFee;
                order.PaymentMethod = model.PaymentMethod;
                order.Notes = model.Notes;
                order.TotalAmount = _context.OrderDetails
                    .Where(d => d.OrderId == order.Id)
                    .Sum(d => d.Quantity * d.UnitPrice) + order.ShippingFee;

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
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int status)
        {
            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            if (status != 0 && status != 1 && status != 3)
            {
                TempData["Error"] = "Trạng thái không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            if (order.Status == status)
            {
                return RedirectToAction(nameof(Index));
            }

            if (order.Status == 0 && status == 1)
            {
                order.Status = 1;
                _context.SaveChanges();
                await SendOrderStatusEmailAsync(order, "Đơn hàng đã được duyệt");
                TempData["Success"] = $"Đã duyệt đơn hàng #{order.Id}";
            }
            else if (order.Status == 0 && status == 3)
            {
                RestoreOrderStock(order);
                order.Status = 3;
                _context.SaveChanges();
                await SendOrderStatusEmailAsync(order, "Đơn hàng đã bị từ chối");
                TempData["Success"] = $"Đã từ chối đơn hàng #{order.Id}";
            }
            else
            {
                TempData["Error"] = "Chỉ có thể duyệt hoặc từ chối đơn hàng đang chờ duyệt";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var order = _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.Status == 0)
            {
                order.Status = 1;
                _context.SaveChanges();
                await SendOrderStatusEmailAsync(order, "Đơn hàng đã được duyệt");
                TempData["Success"] = $"Đã duyệt đơn hàng #{order.Id}";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.Status != 3)
            {
                RestoreOrderStock(order);
                order.Status = 3;
                _context.SaveChanges();
                await SendOrderStatusEmailAsync(order, "Đơn hàng đã bị từ chối");
                TempData["Success"] = $"Đã từ chối đơn hàng #{order.Id}";
            }

            return RedirectToAction(nameof(Details), new { id });
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

        private void RestoreOrderStock(Order order)
        {
            foreach (var detail in order.OrderDetails ?? new List<OrderDetail>())
            {
                if (detail.Product == null)
                {
                    continue;
                }

                detail.Product.StockQuantity += detail.Quantity;
                detail.Product.SoldQuantity = Math.Max(
                    0,
                    detail.Product.SoldQuantity - detail.Quantity
                );

                if (detail.IsFlashSale)
                {
                    var flashSaleItem = _context.FlashSaleItems
                        .Include(x => x.FlashSale)
                        .FirstOrDefault(x =>
                            x.ProductId == detail.ProductId &&
                            x.FlashSale != null &&
                            x.FlashSale.StartTime <= order.OrderDate &&
                            x.FlashSale.EndTime >= order.OrderDate);

                    if (flashSaleItem != null)
                    {
                        flashSaleItem.SoldQuantity = Math.Max(
                            0,
                            flashSaleItem.SoldQuantity - detail.Quantity
                        );
                    }
                }
            }
        }

        private async Task SendOrderStatusEmailAsync(Order order, string title)
        {
            var receiveEmail = !string.IsNullOrWhiteSpace(order.ReceiverEmail)
                ? order.ReceiverEmail
                : order.Customer?.Email;

            if (string.IsNullOrWhiteSpace(receiveEmail))
            {
                return;
            }

            var statusText = GetStatusText(order.Status);
            var receiverName = order.ReceiverName ?? order.Customer?.FullName ?? "quý khách";

            var emailBody = $@"
                <h2 style='color:#ef3f84'>LOI Cosmetics - {title}</h2>
                <p>Xin chào <strong>{receiverName}</strong>,</p>
                <p>Đơn hàng <strong>#{order.Id}</strong> của bạn hiện có trạng thái: <strong>{statusText}</strong>.</p>
                <p>Ngày đặt: {order.OrderDate:dd/MM/yyyy HH:mm}</p>
                <p>Tổng tiền: {order.TotalAmount:N0} đ</p>
                <p>Cảm ơn bạn đã sử dụng LOI Cosmetics.</p>
            ";

            try
            {
                await _emailService.SendEmailAsync(
                    receiveEmail,
                    $"LOI Cosmetics - {title} #{order.Id}",
                    emailBody
                );
            }
            catch
            {
                // Không làm thất bại thao tác quản trị nếu gửi email lỗi
            }
        }

        private string GetStatusText(int status)
        {
            return status switch
            {
                0 => "Chờ duyệt",
                1 => "Đã duyệt / Đang giao",
                2 => "Hoàn thành",
                3 => "Từ chối",
                _ => "Không xác định"
            };
        }
    }
}
