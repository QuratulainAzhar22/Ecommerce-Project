namespace MyEcommerceApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public string? Category { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = "/images/default.jpg";
    public int Stock { get; set; }
}