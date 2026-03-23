using Microsoft.AspNetCore.Mvc;
using MyEcommerceApp.Models;
using System.Text.Json;
using MyEcommerceApp.Data;// You might need to install this via NuGet

public class CartController : Controller
{
    private readonly ApplicationDbContext _db;

    public CartController(ApplicationDbContext db)
    {
        _db = db;
    }

    // 1. Display the Cart Page
    public IActionResult Index()
    {
        var cart = GetCartFromSession();
        return View(cart);
    }

    // 2. Add Product to Cart
    public IActionResult AddToCart(int id)
    {
        var product = _db.Products.Find(id);
        if (product == null) return NotFound();

        var cart = GetCartFromSession();
        var cartItem = cart.FirstOrDefault(c => c.ProductId == id);

        if (cartItem == null) {
            cart.Add(new CartItem {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = 1
            });
        } else {
            cartItem.Quantity++;
        }

        SaveCartToSession(cart);
        return RedirectToAction("Index");
    }

    // 3. Remove Item
    public IActionResult Remove(int id)
    {
        var cart = GetCartFromSession();
        var item = cart.FirstOrDefault(c => c.ProductId == id);
        if (item != null) cart.Remove(item);

        SaveCartToSession(cart);
        return RedirectToAction("Index");
    }

    // Helper Methods for Session
    private List<CartItem> GetCartFromSession()
    {
    var sessionData = HttpContext.Session.GetString("Cart");
    
    // Use JsonSerializer.Deserialize instead of JsonConvert.DeserializeObject
    return string.IsNullOrEmpty(sessionData) 
        ? new List<CartItem>() 
        : JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();
}

private void SaveCartToSession(List<CartItem> cart)
{
    // Use JsonSerializer.Serialize instead of JsonConvert.SerializeObject
    var sessionData = JsonSerializer.Serialize(cart);
    HttpContext.Session.SetString("Cart", sessionData);
}
// Move from Cart to Saved for Later
public IActionResult SaveForLater(int id)
{
    var cart = GetCartFromSession();
    var item = cart.FirstOrDefault(x => x.ProductId == id);

    if (item != null)
    {
        // 1. Remove from Cart
        cart.Remove(item);
        SaveCartToSession(cart);

        // 2. Add to Saved List
        var savedList = GetSavedFromSession();
        if (!savedList.Any(x => x.ProductId == id))
        {
            savedList.Add(item);
            SaveSavedToSession(savedList);
        }
    }
    return RedirectToAction("Index");
}

// Helper methods for the "Saved" list (similar to Cart)
private List<CartItem> GetSavedFromSession()
{
    string? data = HttpContext.Session.GetString("SavedItems");
    
    // 1. Check if string is null/empty
    if (string.IsNullOrEmpty(data))
    {
        return new List<CartItem>();
    }

    // 2. Deserialize and use ?? to ensure we never return a null list
    return JsonSerializer.Deserialize<List<CartItem>>(data) ?? new List<CartItem>();
}

private void SaveSavedToSession(List<CartItem> saved)
{
    HttpContext.Session.SetString("SavedItems", JsonSerializer.Serialize(saved));
}
// Inside Controllers/CartController.cs

public IActionResult Checkout()
{
    // Ensure you are using the correct property name from your Model
    var itemsInCart = _db.CartItems.Where(c => c.IsSavedForLater == false).ToList();

    // if (!itemsInCart.Any())
    // {
    //     return RedirectToAction("Index");
    // }

    // // Clear the items
    // _db.CartItems.RemoveRange(itemsInCart);
    // _db.SaveChanges(); // If this fails, the redirect won't happen

    return RedirectToAction("OrderSuccess");
}
public IActionResult OrderSuccess()
{
    return View(); // You'll need to create an OrderSuccess.cshtml view
}
}
