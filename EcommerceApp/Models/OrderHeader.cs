namespace MyEcommerceApp.Models;
public class OrderHeader {
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string? OrderStatus { get; set; } // e.g., "Placed", "Shipped"
    // Add User ID here if you have Login setup
}