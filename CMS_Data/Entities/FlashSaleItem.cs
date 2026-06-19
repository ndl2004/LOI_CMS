using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Entities
{
    public class FlashSaleItem
    {
        [Key]
        public int Id { get; set; }

        public int FlashSaleId { get; set; }

        public int ProductId { get; set; }

        [Range(1, 100, ErrorMessage = "Phần trăm giảm giá phải từ 1 đến 100")]
        public int DiscountPercent { get; set; }

        public int SaleQuantity { get; set; }

        public int SoldQuantity { get; set; }

        [ForeignKey("FlashSaleId")]
        public virtual FlashSale? FlashSale { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
