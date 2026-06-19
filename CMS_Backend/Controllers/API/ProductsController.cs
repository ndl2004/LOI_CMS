using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace CMS_Backend.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _context.Products
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.ImageUrl,
                    p.CategoryProductId,
                    p.SoldQuantity,
                    p.StockQuantity
                })
                .ToList();

            return Ok(products);
        }

        [HttpGet("category/{categoryProductId}")]
        public IActionResult GetByCategory(int categoryProductId)
        {
            var products = _context.Products
                .Where(p => p.CategoryProductId == categoryProductId)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.ImageUrl,
                    p.CategoryProductId,
                    p.SoldQuantity,
                    p.StockQuantity
                })
                .ToList();

            return Ok(products);
        }

        [HttpGet("search")]
        public IActionResult Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return Ok(new List<object>());
            }

            keyword = keyword.Trim();

            var products = _context.Products
                .Where(p =>
                    p.Name.Contains(keyword) ||
                    (p.Description != null && p.Description.Contains(keyword))
                )
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.ImageUrl,
                    p.CategoryProductId,
                    p.SoldQuantity,
                    p.StockQuantity
                })
                .ToList();

            return Ok(products);
        }

        [HttpGet("filter-price")]
        public IActionResult FilterByPrice(
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            var products = query
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.ImageUrl,
                    p.CategoryProductId,
                    p.SoldQuantity,
                    p.StockQuantity
                })
                .ToList();

            return Ok(products);
        }

        [HttpGet("filter")]
        public IActionResult Filter(
            [FromQuery] string? keyword,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int? categoryProductId)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                query = query.Where(p =>
                    p.Name.Contains(keyword) ||
                    (p.Description != null && p.Description.Contains(keyword))
                );
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (categoryProductId.HasValue && categoryProductId.Value > 0)
            {
                query = query.Where(p =>
                    p.CategoryProductId == categoryProductId.Value
                );
            }

            var products = query
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.ImageUrl,
                    p.CategoryProductId,
                    p.SoldQuantity,
                    p.StockQuantity
                })
                .ToList();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var now = DateTime.Now;

            var productEntity = _context.Products
                .Include(p => p.CategoryProduct)
                .Where(p => p.Id == id)
                .FirstOrDefault();

            if (productEntity == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy sản phẩm trong hệ thống"
                });
            }

            var flashSaleItem = _context.FlashSaleItems
                .Include(x => x.FlashSale)
                .FirstOrDefault(x =>
                    x.ProductId == id &&
                    x.FlashSale != null &&
                    x.FlashSale.IsActive &&
                    x.FlashSale.StartTime <= now &&
                    x.FlashSale.EndTime >= now);

            object? flashSale = null;

            if (flashSaleItem != null)
            {
                var remainingQuantity = flashSaleItem.SaleQuantity == 0
                    ? (int?)null
                    : Math.Max(0, flashSaleItem.SaleQuantity - flashSaleItem.SoldQuantity);

                flashSale = new
                {
                    flashSaleItem.Id,
                    flashSaleItem.DiscountPercent,
                    flashSaleItem.SaleQuantity,
                    flashSaleItem.SoldQuantity,
                    RemainingQuantity = remainingQuantity,
                    IsSoldOut = remainingQuantity == 0,
                    SalePrice = productEntity.Price - (productEntity.Price * flashSaleItem.DiscountPercent / 100),
                    flashSaleItem.FlashSale!.EndTime
                };
            }

            var product = new
            {
                productEntity.Id,
                productEntity.Name,
                productEntity.Description,
                productEntity.Price,
                productEntity.StockQuantity,
                productEntity.ImageUrl,
                productEntity.CategoryProductId,
                productEntity.SoldQuantity,

                CategoryName = productEntity.CategoryProduct != null
                    ? productEntity.CategoryProduct.Name
                    : "Chưa có danh mục",

                FlashSale = flashSale
            };

            return Ok(product);
        }
    }
}
