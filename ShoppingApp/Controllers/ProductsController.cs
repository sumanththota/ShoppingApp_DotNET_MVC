using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Data;
using ShoppingApp.Models;

namespace MyApp.Controllers;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var product = await _context.Products.ToListAsync();
        return View(product);
    }
    
    // CRUD actions...
    
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("Id, Name, Price, Quantity, Category")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([Bind("Id,Name,Price,Quantity,Category")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}