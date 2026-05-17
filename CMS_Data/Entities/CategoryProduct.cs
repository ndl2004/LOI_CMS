/*Họ tên: Nguyễn Đình Lợi       
 *MSSV: 2122110147
 *Lớp: CCQ2211D
 *Ngày tạo: 17/5/2026
 *Mô tả: Lớp CategoryProduct đại diện cho một danh mục sản phẩm trong hệ thống quản lý nội dung (CMS).
 * 
 */
using System.ComponentModel.DataAnnotations;
namespace CMS.Data.Entities
{

    public class CategoryProduct
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        // Quan hệ: Một danh mục có nhiều sản phẩm
        public virtual ICollection<Product>? Products { get; set; }
    }
}

