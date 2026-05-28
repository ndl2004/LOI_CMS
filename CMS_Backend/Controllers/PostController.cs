using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // Cần thêm namespace này
namespace CMS.Backend.Controllers
{
    [Authorize] // Bắt buộc phải đăng nhập mới được vào các hàm bên dưới
    public class PostController : Controller
    {
        // Khai báo DbContext để làm việc với Database
        private readonly ApplicationDbContext _context;

        // Constructor: nhận ApplicationDbContext thông qua Dependency Injection
        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách bài viết
        // id: nếu có truyền id thì lọc bài viết theo CategoryId
        public IActionResult Index(int? id)
        {
            // Include(p => p.Category): lấy thêm dữ liệu danh mục của bài viết
            // AsQueryable(): giúp có thể nối thêm điều kiện LINQ phía sau
            var posts = _context.Posts
                .Include(p => p.Category)
                .AsQueryable();

            // Nếu URL có id, lọc bài viết theo danh mục
            // Ví dụ: /Post/Index/1 hoặc /Post?id=1
            if (id != null)
            {
                posts = posts.Where(p => p.CategoryId == id);
            }

            // Sắp xếp bài viết mới nhất lên đầu
            var result = posts
                .OrderByDescending(p => p.CreatedDate)
                .ToList();

            // Trả dữ liệu sang View Index.cshtml
            return View(result);
        }

        // Hiển thị chi tiết một bài viết
        // Ví dụ URL: /Post/Details/5
        public IActionResult Details(int id)
        {
            // Tìm bài viết theo Id
            // Include(p => p.Category): lấy thêm tên danh mục để hiển thị trong Details
            var post = _context.Posts
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            // Nếu không tìm thấy bài viết thì trả về lỗi 404
            if (post == null)
                return NotFound();

            // Trả dữ liệu bài viết sang View Details.cshtml
            return View(post);
        }

        // Hiển thị form tạo mới bài viết
        // GET: /Post/Create
        [HttpGet]
        public IActionResult Create()
        {
            // Lấy danh sách Category từ database
            // SelectList dùng để đổ dữ liệu vào dropdown trong View
            ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name");

            return View();
        }

        // Xử lý lưu bài viết mới vào database
        // POST: /Post/Create
        [HttpPost]
        public IActionResult Create(Post model, IFormFile uploadImage)
        {
            // Kiểm tra người dùng có upload ảnh hay không
            if (uploadImage != null && uploadImage.Length > 0)
            {
                // Tạo đường dẫn thư mục lưu ảnh: wwwroot/uploads
                string folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads"
                );

                // Nếu thư mục uploads chưa tồn tại thì tạo mới
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // Tạo tên file duy nhất để tránh trùng tên ảnh
                string fileName = Guid.NewGuid().ToString()
                                  + Path.GetExtension(uploadImage.FileName);

                // Đường dẫn vật lý để lưu file vào máy
                string filePath = Path.Combine(folder, fileName);

                // Lưu file ảnh vào thư mục uploads
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadImage.CopyTo(stream);
                }

                // Lưu đường dẫn tương đối vào database
                // Ví dụ: /uploads/abc123.jpg
                model.ImageUrl = "/uploads/" + fileName;
            }

            // Thêm bài viết mới vào bảng Posts
            _context.Posts.Add(model);

            // Lưu thay đổi xuống SQL Server
            _context.SaveChanges();

            // Sau khi thêm xong, quay về trang danh sách bài viết
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            // 1. Tìm bài viết theo Id
            var post = _context.Posts.Find(id);

            if (post != null)
            {
                // 2. Xóa khỏi bộ nhớ tạm
                _context.Posts.Remove(post);

                // 3. Cập nhật xuống SQL Server
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // GET: Hiển thị form kèm dữ liệu cũ
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            // Chuẩn bị lại danh sách danh mục để người dùng có thể đổi chuyên mục
            ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        // POST: Thực hiện cập nhật
        [HttpPost]
        public IActionResult Edit(Post model, IFormFile uploadImage)
        {
            // Bước 1: Kiểm tra xem người dùng có chọn file ảnh mới không
            if (uploadImage != null && uploadImage.Length > 0)
            {
                // Thực hiện quy trình upload giống như trang Create
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadImage.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadImage.CopyTo(stream);
                }

                // Cập nhật đường dẫn ảnh mới vào model
                model.ImageUrl = "/uploads/" + fileName;
            }
            else
            {
                // Bước quan trọng: Nếu không upload ảnh mới, chúng ta phải giữ lại ảnh cũ
                // Chúng ta cần lấy lại giá trị ImageUrl từ Database để tránh bị ghi đè thành rỗng
                var oldPost = _context.Posts.AsNoTracking().FirstOrDefault(p => p.Id == model.Id);
                if (oldPost != null && string.IsNullOrEmpty(model.ImageUrl))
                {
                    model.ImageUrl = oldPost.ImageUrl;
                }
            }
            _context.Posts.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}