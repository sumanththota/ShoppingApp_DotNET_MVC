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
    private string sessionId { get; set; }

    public CartController(ILogger<CartController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        sessionId = Request.Cookies["cartSessionId"];
        
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.SessionId == sessionId);
      
        
        List<CartItem> cartItems = await _context.CartItems.Where(ci => ci.CartId == cart.Id).ToListAsync();
        
      
        
        return View(cartItems);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity,decimal price, string name, string sessionId)
    {

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

        return RedirectToAction("Index", "Shop");
    }
    [HttpPost]
    public async Task<IActionResult> Update(int productId, int quantity)
    {
        // Retrieve the session ID from the request cookies
        if (string.IsNullOrEmpty(sessionId))
        {
            return RedirectToAction("Index");
        }
        // Fetch the cart using the session ID
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.SessionId == sessionId);

        if (cart == null)
        {
            return RedirectToAction("Index");
        }

        // Retrieve the existing item by productId
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        
        if (existingItem == null)
        {
            return RedirectToAction("Index");
        }

        // Update the item's quantity
        existingItem.Quantity += quantity;

        // If the updated quantity is zero or less, remove the item from the cart
        if (existingItem.Quantity <= 0)
        {
             cart.Items.Remove(existingItem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index"); 
    }

}