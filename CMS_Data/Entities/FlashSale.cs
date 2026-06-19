using System.ComponentModel.DataAnnotations;

namespace CMS.Data.Entities
{
    public class FlashSale
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên chương trình không được để trống")]
        [StringLength(150)]
        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<FlashSaleItem>? FlashSaleItems { get; set; }
    }
}
