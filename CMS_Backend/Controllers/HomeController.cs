/*Họ tên: Nguyễn Đình Lợi       
 *MSSV: 2122110147
 *Lớp: CCQ2211D
 *Ngày tạo: 17/5/2026
 *Mô tả: Lớp HomeController 
 * 
 */
using CMS_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMS_Backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
