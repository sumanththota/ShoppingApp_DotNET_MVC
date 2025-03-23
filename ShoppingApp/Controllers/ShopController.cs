using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Models;
using ShoppingApp.Data;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<ShopController> _logger;

        private readonly AppDbContext _context;

        public ShopController(ILogger<ShopController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(Category? category)
        {

            var products = category == null
                ? _context.Products.ToList()
                : _context.Products.Where(product => product.Category == category).ToList();

            ViewBag.Categories = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
            return View(products);
        }

        // Other actions...
    }
}