using Microsoft.AspNetCore.Mvc;
using MyEcommerceApp.Models;
using Microsoft.EntityFrameworkCore;
using MyEcommerceApp.Data;

namespace MyEcommerceApp.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

        // 3. Inject the context through the Constructor
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
   

    public async Task<IActionResult> Index()
{
    var allProducts = await _context.Products.ToListAsync();

    var viewModel = new HomeViewModel
    {
        // Get unique category names from the Product table
        Categories = allProducts.Select(p => p.Category).Distinct().Where(c => c != null)!,

        DealProducts = allProducts.Take(5),

        // Filter by the string name in your model
        HomeProducts = allProducts.Where(p => p.Category == "Home interiors").Take(8),
        
        ElectronicsProducts = allProducts.Where(p => p.Category == "Computer and tech").Take(8),

        RecommendedProducts = allProducts.OrderBy(r => Guid.NewGuid()).Take(10)
    };

    return View(viewModel);
}
}

    
