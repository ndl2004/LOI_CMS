using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Backend.Controllers
{
    public class CategoryProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CategoryProductController(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var data = _context.CategoriesProducts.ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryProduct model, IFormFile? ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                model.ImageUrl = UploadCategoryImage(ImageFile);
            }

            if (ModelState.IsValid)
            {
                _context.CategoriesProducts.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var categoryProduct = _context.CategoriesProducts.Find(id);

            if (categoryProduct == null)
            {
                return NotFound();
            }

            return View(categoryProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryProduct model, IFormFile? ImageFile)
        {
            var existingCategory = _context.CategoriesProducts.Find(model.Id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = model.Name;
            existingCategory.Description = model.Description;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                existingCategory.ImageUrl = UploadCategoryImage(ImageFile);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var categoryProduct = _context.CategoriesProducts.Find(id);

            if (categoryProduct == null)
            {
                return NotFound();
            }

            return View(categoryProduct);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var categoryProduct = _context.CategoriesProducts.Find(id);

            if (categoryProduct != null)
            {
                _context.CategoriesProducts.Remove(categoryProduct);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private string UploadCategoryImage(IFormFile imageFile)
        {
            string uploadFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "categories"
            );

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            string fileName =
                Guid.NewGuid().ToString() +
                Path.GetExtension(imageFile.FileName);

            string filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return "/uploads/categories/" + fileName;
        }
    }
}