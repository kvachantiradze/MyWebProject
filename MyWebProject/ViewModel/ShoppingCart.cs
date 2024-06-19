using MyWebProject.Models;

namespace MyWebProject.ViewModel
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal TotalPrice => Items.Sum(item => item.Product.Price * item.Quantity);
    }
}
