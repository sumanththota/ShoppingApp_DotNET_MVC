using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
    private readonly AppSettings _appSettings;
    private string sessionId { get; set; }
    
    public CheckoutController(ILogger<CheckoutController> logger, ApplicationDbContext context,IOptions<AppSettings> appSettings)
    {
        _logger = logger;
        _context = context;
        _appSettings = appSettings.Value;
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
        
        // Calculate the total amount of the cart items
        decimal total = cartItems.Sum(item => item.Price * item.Quantity);
        
        _logger.LogInformation("Creating checkout session");
        var domain = _appSettings.DomainUrl;
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
            ReturnUrl = Url.Action("Return", "Checkout", new { session_id = "{CHECKOUT_SESSION_ID}" }, Request.Scheme)
        };
        var service = new SessionService();
        Session session = service.Create(options);

        return Json (new {clientSecret = session.ClientSecret});
    }
    
    [Route("session-status")]
    [ApiController]
    public class SessionStatusController : Controller
    {
        [HttpGet]
        public ActionResult SessionStatus([FromQuery] string session_id)
        {
            var sessionService = new SessionService();
            Session session = sessionService.Get(session_id);

            return Json(new {status = session.Status,  customer_email = session.CustomerDetails.Email});
        }
    }
    [HttpGet("return")]
    public IActionResult Return(string session_id)
    {
        ViewBag.SessionId = session_id;
        return View("return");
    }
}