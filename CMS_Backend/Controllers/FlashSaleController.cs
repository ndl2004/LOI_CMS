using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers
{
    [Authorize]
    public class FlashSaleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlashSaleController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.FlashSales
                .Include(x => x.FlashSaleItems)
                .OrderByDescending(x => x.StartTime)
                .ToList();

            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new FlashSale
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2),
                IsActive = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FlashSale model)
        {
            if (model.EndTime <= model.StartTime)
            {
                ModelState.AddModelError("EndTime", "Thời gian kết thúc phải sau thời gian bắt đầu");
            }

            if (model.IsActive &&
                HasOverlappingActiveFlashSale(model.StartTime, model.EndTime))
            {
                ModelState.AddModelError(
                    "StartTime",
                    "Đã có Flash Sale active trong khoảng thời gian này. Vui lòng tắt chương trình cũ hoặc chọn thời gian khác."
                );
            }

            if (ModelState.IsValid)
            {
                _context.FlashSales.Add(model);
                _context.SaveChanges();

                return RedirectToAction(nameof(Edit), new { id = model.Id });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var flashSale = _context.FlashSales
                .Include(x => x.FlashSaleItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.Id == id);

            if (flashSale == null)
            {
                return NotFound();
            }

            LoadProductList();
            return View(flashSale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FlashSale model)
        {
            var flashSale = _context.FlashSales.Find(model.Id);

            if (flashSale == null)
            {
                return NotFound();
            }

            if (model.EndTime <= model.StartTime)
            {
                ModelState.AddModelError("EndTime", "Thời gian kết thúc phải sau thời gian bắt đầu");
            }

            if (model.IsActive &&
                HasOverlappingActiveFlashSale(model.StartTime, model.EndTime, model.Id))
            {
                ModelState.AddModelError(
                    "StartTime",
                    "Đã có Flash Sale active trong khoảng thời gian này. Vui lòng tắt chương trình cũ hoặc chọn thời gian khác."
                );
            }

            if (!ModelState.IsValid)
            {
                LoadProductList();
                model.FlashSaleItems = _context.FlashSaleItems
                    .Include(x => x.Product)
                    .Where(x => x.FlashSaleId == model.Id)
                    .ToList();

                return View(model);
            }

            flashSale.Name = model.Name;
            flashSale.StartTime = model.StartTime;
            flashSale.EndTime = model.EndTime;
            flashSale.IsActive = model.IsActive;

            _context.SaveChanges();

            return RedirectToAction(nameof(Edit), new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddItems(
            int flashSaleId,
            List<int> productIds,
            int discountPercent,
            int saleQuantity)
        {
            var flashSale = _context.FlashSales.Find(flashSaleId);

            if (flashSale == null)
            {
                return NotFound();
            }

            if (productIds == null || productIds.Count == 0)
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một sản phẩm";
                return RedirectToAction(nameof(Edit), new { id = flashSaleId });
            }

            if (discountPercent < 1 || discountPercent > 100)
            {
                TempData["Error"] = "Phần trăm giảm giá phải từ 1 đến 100";
                return RedirectToAction(nameof(Edit), new { id = flashSaleId });
            }

            if (saleQuantity < 0)
            {
                TempData["Error"] = "Số lượng sale không được nhỏ hơn 0";
                return RedirectToAction(nameof(Edit), new { id = flashSaleId });
            }

            var selectedProducts = _context.Products
                .Where(x => productIds.Distinct().Contains(x.Id))
                .ToList();

            if (saleQuantity > 0)
            {
                var invalidProduct = selectedProducts
                    .FirstOrDefault(x => saleQuantity > x.StockQuantity);

                if (invalidProduct != null)
                {
                    TempData["Error"] = $"Số lượng sale của {invalidProduct.Name} không được vượt quá tồn kho ({invalidProduct.StockQuantity})";
                    return RedirectToAction(nameof(Edit), new { id = flashSaleId });
                }
            }

            foreach (var productId in productIds.Distinct())
            {
                var exists = _context.FlashSaleItems.Any(x =>
                    x.FlashSaleId == flashSaleId &&
                    x.ProductId == productId);

                if (exists)
                {
                    continue;
                }

                _context.FlashSaleItems.Add(new FlashSaleItem
                {
                    FlashSaleId = flashSaleId,
                    ProductId = productId,
                    DiscountPercent = discountPercent,
                    SaleQuantity = saleQuantity,
                    SoldQuantity = 0
                });
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Edit), new { id = flashSaleId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateItem(
            int id,
            int discountPercent,
            int saleQuantity)
        {
            var item = _context.FlashSaleItems
                .Include(x => x.Product)
                .FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            if (discountPercent < 1 || discountPercent > 100)
            {
                TempData["Error"] = "Phần trăm giảm giá phải từ 1 đến 100";
                return RedirectToAction(nameof(Edit), new { id = item.FlashSaleId });
            }

            if (saleQuantity < 0)
            {
                TempData["Error"] = "Số lượng sale không được nhỏ hơn 0";
                return RedirectToAction(nameof(Edit), new { id = item.FlashSaleId });
            }

            if (saleQuantity > 0 && item.Product != null && saleQuantity > item.Product.StockQuantity)
            {
                TempData["Error"] = $"Số lượng sale của {item.Product.Name} không được vượt quá tồn kho ({item.Product.StockQuantity})";
                return RedirectToAction(nameof(Edit), new { id = item.FlashSaleId });
            }

            if (saleQuantity > 0 && saleQuantity < item.SoldQuantity)
            {
                TempData["Error"] = $"Số lượng sale không được nhỏ hơn số đã bán ({item.SoldQuantity})";
                return RedirectToAction(nameof(Edit), new { id = item.FlashSaleId });
            }

            item.DiscountPercent = discountPercent;
            item.SaleQuantity = saleQuantity;

            _context.SaveChanges();

            TempData["Success"] = "Đã cập nhật sản phẩm Flash Sale";
            return RedirectToAction(nameof(Edit), new { id = item.FlashSaleId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveItem(int id)
        {
            var item = _context.FlashSaleItems.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            var flashSaleId = item.FlashSaleId;

            _context.FlashSaleItems.Remove(item);
            _context.SaveChanges();

            return RedirectToAction(nameof(Edit), new { id = flashSaleId });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var flashSale = _context.FlashSales
                .Include(x => x.FlashSaleItems)
                .FirstOrDefault(x => x.Id == id);

            if (flashSale == null)
            {
                return NotFound();
            }

            return View(flashSale);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var flashSale = _context.FlashSales.Find(id);

            if (flashSale != null)
            {
                _context.FlashSales.Remove(flashSale);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadProductList()
        {
            ViewBag.Products = new MultiSelectList(
                _context.Products.OrderBy(x => x.Name).ToList(),
                "Id",
                "Name"
            );
        }

        private bool HasOverlappingActiveFlashSale(
            DateTime startTime,
            DateTime endTime,
            int? ignoreFlashSaleId = null)
        {
            return _context.FlashSales.Any(x =>
                x.IsActive &&
                (!ignoreFlashSaleId.HasValue || x.Id != ignoreFlashSaleId.Value) &&
                startTime < x.EndTime &&
                endTime > x.StartTime);
        }
    }
}
