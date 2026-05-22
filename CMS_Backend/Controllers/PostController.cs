/*Họ tên: Nguyễn Đình Lợi       
 *MSSV: 2122110147
 *Lớp: CCQ2211D
 *Ngày tạo: 17/5/2026
 *Mô tả: Lớp PostController đại diện cho một controller trong hệ thống quản lý nội dung (CMS) để quản lý các bài viết (posts).
 * 
 */
using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Backend.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var posts = _context.Posts.ToList();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);

            if (post == null)
                return NotFound();

            return View(post);
        }
    }
}