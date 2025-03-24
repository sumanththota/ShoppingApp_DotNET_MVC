using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace ShoppingApp.Controllers;

[Route("create-checkout-session")]
[ApiController]
public class CheckoutController : Controller
{
    private readonly ILogger<CheckoutController> _logger;
    
    public CheckoutController(ILogger<CheckoutController> logger)
    {
        _logger = logger;
    }
    public IActionResult Checkout()
    {
        return View();
    }
  
    [HttpPost]
    public ActionResult Create()
    {
        
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
                        UnitAmount = 2500, // amount in cents
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