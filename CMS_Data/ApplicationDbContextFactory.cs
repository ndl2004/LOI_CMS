/*Họ tên: Nguyễn Đình Lợi       
 *MSSV: 2122110147
 *Lớp: CCQ2211D
 *Ngày tạo: 17/5/2026
 *Mô tả: Lớp ApplicationDbContextFactory đại diện cho một factory để tạo instance của ApplicationDbContext trong thời gian thiết kế (design time) khi sử dụng Entity Framework Core. Điều này thường được sử dụng để hỗ trợ các công cụ như migrations khi chúng cần một instance của DbContext để thực hiện các tác vụ liên quan đến cơ sở dữ liệu.
 * 
 */
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CMS.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=.;Database=LOI_CMS_DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}