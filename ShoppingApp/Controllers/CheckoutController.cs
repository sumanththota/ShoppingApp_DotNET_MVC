using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Data;
using ShoppingApp.Models;
using Stripe.Checkout;

namespace ShoppingApp.Controllers;

[Route("create-checkout-session")]
[ApiController]
public class CheckoutController : Controller
{
    private readonly ILogger<CheckoutController> _logger;
    private ApplicationDbContext _context;
    private string sessionId { get; set; }
    
    public CheckoutController(ILogger<CheckoutController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public IActionResult Checkout()
    {
        return View();
    }
  
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        sessionId = Request.Cookies["cartSessionId"];
        
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        
        List<CartItem> cartItems = await _context.CartItems.Where(ci => ci.CartId == cart.Id).ToListAsync();
        decimal total = cartItems.Sum(item => item.Price * item.Quantity);
        
        _logger.LogInformation("Creating checkout session");
        var domain = "http://localhost:4242";
        var options = new SessionCreateOptions
        {
            UiMode = "embedded",
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(total * 100), // amount in cents
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Shopping Cart Items"
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            ReturnUrl = domain + "/return.html?session_id={CHECKOUT_SESSION_ID}",
        };
        var service = new SessionService();
        Session session = service.Create(options);

        return Json (new {clientSecret = session.ClientSecret});
    }
}