using exercise.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace exercise.Controllers
{
    public class ShopController:Controller
    {
        private readonly ILogger<ShopController> _logger;

        public ShopController(ILogger<ShopController> logger)
        {
            _logger = logger;
        }
        public IActionResult Shop()
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
