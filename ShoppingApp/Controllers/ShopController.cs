using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Models;
using ShoppingApp.Data;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<ShopController> _logger;

        private readonly ApplicationDbContext _context;

        public ShopController(ILogger<ShopController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(Category? category)
        {

            // Fetch all products from the database
            var products = category == null
                ? _context.Products.ToList()
                : _context.Products.Where(product => product.Category == category).ToList();

            // Pass the list of categories to the view
            ViewBag.Categories = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
            return View(products);
        }

    }
}