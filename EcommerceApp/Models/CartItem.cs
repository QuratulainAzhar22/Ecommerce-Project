namespace MyEcommerceApp.Models;

public class CartItem
{
    public int Id { get; set; } // Database Primary Key
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }

    // Calculated property (not stored in DB)
    public decimal TotalPrice => Price * Quantity;
   public bool IsSavedForLater { get; set; } = false;
}

