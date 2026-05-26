using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
        {
            var posts = _context.Posts
                .Include(p => p.Category)
                .AsQueryable();

            if (id != null)
            {
                posts = posts.Where(p => p.CategoryId == id);
            }

            var result = posts
                .OrderByDescending(p => p.CreatedDate)
                .ToList();

            return View(result);
        }

        // GET: Post/Details/5
        public IActionResult Details(int id)
        {
            var post = _context.Posts
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
                return NotFound();

            return View(post);
        }

    }
}