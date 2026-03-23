using Microsoft.AspNetCore.Mvc;
using MyEcommerceApp.Models; // Ensure this matches your namespace
using MyEcommerceApp.Data;

namespace MyEcommerceApp.Controllers;

public class FrontendController : Controller
{
    private readonly ApplicationDbContext _context;

        // 2. You MUST create a Constructor to "fill" that variable
        public FrontendController(ApplicationDbContext context)
        {
            _context = context;
        }

    public IActionResult Secondpage() // Make sure this name matches your .cshtml file name
{
    // 1. Fetch the data
    var productList = _context.Products.ToList();

    // 2. PASS THE DATA TO THE VIEW (Crucial Step!)
    // If this is empty like "return View();", you will get that Null error.
    return View(productList); 
}
 public IActionResult Homepage_a()
    {
        var productList = _context.Products.ToList();
        return View(productList);
    }

    public IActionResult Homepage()
    {
        var productList = _context.Products.ToList();
        return View(productList);
    }
    
   // This goes in your Controller
public IActionResult ThirdPage(int id)
{
    // 1. Fetch the single product from the database that matches the ID
    var product = _context.Products.FirstOrDefault(u => u.Id == id);

    // 2. Safety check: If no product is found, show an error or 404
    if (product == null)
    {
        return NotFound();
    }
    // 2. Get Suggestions (Right Sidebar)
    ViewBag.Suggestions = _context.Products.Where(p => p.Id != id).Take(4).ToList();

    // 3. Get Related Products (Bottom Row)
    ViewBag.RelatedProducts = _context.Products.Where(p => p.Id != id).Take(6).ToList();
    //ViewBag.Suggestions = _db.Products.Where(x => x.Id != id).Take(4).ToList();
    // 3. Pass that specific product object to the View
    return View(product);
}
}