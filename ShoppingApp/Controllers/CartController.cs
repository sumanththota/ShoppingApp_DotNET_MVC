using Microsoft.AspNetCore.Mvc;
using MyApp.Controllers;
using ShoppingApp.Data;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Models;
using SQLitePCL;

namespace ShoppingApp.Controllers;

public class CartController : Controller
{
   
    private readonly ILogger<CartController> _logger;

    private readonly AppDbContext _context;

    public CartController(ILogger<CartController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        var sessionId = Request.Cookies["cartSessionId"];
        // if (string.IsNullOrEmpty(sessionId))
        // {
        //     return View(new List<CartItem>());
        // }
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        
        List<CartItem> cartItems = await _context.CartItems.Where(ci => ci.CartId == cart.Id).ToListAsync();
        
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity,decimal price, string name, string sessionId)
    {
        _logger.LogInformation("Add to Cart called for ProductId: {ProductId}", productId);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.SessionId == sessionId);

        if (cart == null)
        {
            cart = new Cart
            {
                SessionId = sessionId,
                Items = new List<CartItem>()
            };
            _context.Carts.Add(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Name = name,
                Price = price,
                Quantity = quantity
            });
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("ShopIndex", "Shop");
    }

}