
namespace MyEcommerceApp.Models;

  public class HomeViewModel
{
    
    public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();
    
    public IEnumerable<Product> DealProducts { get; set; } = Enumerable.Empty<Product>();
    public IEnumerable<Product> HomeProducts { get; set; } = Enumerable.Empty<Product>();
    public IEnumerable<Product> ElectronicsProducts { get; set; } = Enumerable.Empty<Product>();
    public IEnumerable<Product> RecommendedProducts { get; set; } = Enumerable.Empty<Product>();
}
