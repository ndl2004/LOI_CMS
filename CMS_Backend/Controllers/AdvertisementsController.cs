using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS_Backend.Controllers
{
    public class AdvertisementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdvertisementsController(
            ApplicationDbContext context,
            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _context.Advertisements
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            Advertisement advertisement,
            IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                advertisement.Image = await UploadBannerImage(imageFile);
            }

            if (string.IsNullOrWhiteSpace(advertisement.Link))
            {
                advertisement.Link = "/shop";
            }

            _context.Advertisements.Add(advertisement);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Advertisements.FindAsync(id);

            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            Advertisement advertisement,
            IFormFile? imageFile,
            bool isActive = false)
        {
            var item = await _context.Advertisements.FindAsync(id);

            if (item == null)
                return NotFound();

            item.Title = advertisement.Title;
            item.Description = advertisement.Description;
            item.SortOrder = advertisement.SortOrder;
            item.IsActive = isActive;

            if (string.IsNullOrWhiteSpace(item.Link))
            {
                item.Link = "/shop";
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                item.Image = await UploadBannerImage(imageFile);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Advertisements.FindAsync(id);

            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Advertisements.FindAsync(id);

            if (item != null)
            {
                _context.Advertisements.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<string> UploadBannerImage(IFormFile imageFile)
        {
            var folder = Path.Combine(
                _env.WebRootPath,
                "images",
                "banner");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var fileName =
                Guid.NewGuid().ToString() +
                Path.GetExtension(imageFile.FileName);

            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return "/images/banner/" + fileName;
        }
    }
}