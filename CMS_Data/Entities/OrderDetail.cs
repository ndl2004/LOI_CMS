/*Họ tên: Nguyễn Đình Lợi       
 *MSSV: 2122110147
 *Lớp: CCQ2211D
 *Ngày tạo: 17/5/2026
 *Mô tả: Lớp OrderDetail đại diện cho chi tiết đơn hàng trong hệ thống quản lý nội dung (CMS).
 * 
 */
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } // Giá tại thời điểm mua

        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
