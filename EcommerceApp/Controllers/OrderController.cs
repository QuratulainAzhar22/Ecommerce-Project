using MyEcommerceApp.Data;
using Microsoft.AspNetCore.Mvc;
using MyEcommerceApp.Models;
using MyEcommerceApp.Controllers;
using System.Text.Json;
public class OrderController : Controller {
    private readonly ApplicationDbContext _db;
    public OrderController(ApplicationDbContext db) => _db = db;

    // This is what the "Orders" button in your navbar will link to
    public IActionResult Index() {
        // Fetch all previous orders
        var orders = _db.OrderHeaders.OrderByDescending(o => o.OrderDate).ToList();
        return View(orders);
    }

    // This is called when the user clicks "Checkout" in the Cart
    public IActionResult PlaceOrder() {
        var cart = GetCartFromSession(); // Your existing method
        
        OrderHeader header = new OrderHeader {
            OrderDate = DateTime.Now,
            TotalPrice = cart.Sum(x => x.Price * x.Quantity),
            OrderStatus = "Placed"
        };
        _db.OrderHeaders.Add(header);
        _db.SaveChanges();

        foreach(var item in cart) {
            _db.OrderDetails.Add(new OrderDetail {
                OrderHeaderId = header.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            });
        }
        _db.SaveChanges();
        HttpContext.Session.Remove("Cart"); // Clear the cart
        return RedirectToAction("Index");
    }
    private List<CartItem> GetCartFromSession()
{
    var sessionData = HttpContext.Session.GetString("Cart");
    
    // Check if the session is empty
    if (string.IsNullOrEmpty(sessionData))
    {
        return new List<CartItem>();
    }

    // Deserialize the JSON string back into a List of CartItems
    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    return JsonSerializer.Deserialize<List<CartItem>>(sessionData, options) ?? new List<CartItem>();
}
}
