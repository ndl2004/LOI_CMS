using CMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS_Backend.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashSalesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FlashSalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("active")]
        public IActionResult GetActive()
        {
            var now = DateTime.Now;

            var flashSale = _context.FlashSales
                .Include(x => x.FlashSaleItems)
                .ThenInclude(x => x.Product)
                .Where(x =>
                    x.IsActive &&
                    x.StartTime <= now &&
                    x.EndTime >= now)
                .OrderBy(x => x.EndTime)
                .FirstOrDefault();

            if (flashSale == null)
            {
                return Ok(null);
            }

            var items = flashSale.FlashSaleItems?
                .Where(x => x.Product != null)
                .Select(x =>
                {
                    var price = x.Product!.Price;
                    var salePrice = price - (price * x.DiscountPercent / 100);
                    var remainingQuantity = x.SaleQuantity == 0
                        ? (int?)null
                        : Math.Max(0, x.SaleQuantity - x.SoldQuantity);

                    return new
                    {
                        x.Id,
                        x.ProductId,
                        x.Product.Name,
                        x.Product.ImageUrl,
                        x.Product.CategoryProductId,
                        Price = price,
                        SalePrice = salePrice,
                        x.DiscountPercent,
                        x.SaleQuantity,
                        x.SoldQuantity,
                        ProductSoldQuantity = x.Product.SoldQuantity,
                        RemainingQuantity = remainingQuantity,
                        IsSoldOut = remainingQuantity == 0
                    };
                })
                .ToList();

            return Ok(new
            {
                flashSale.Id,
                flashSale.Name,
                flashSale.StartTime,
                flashSale.EndTime,
                Items = items
            });
        }
    }
}
