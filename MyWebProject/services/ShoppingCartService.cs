using MyWebProject.Date;
using MyWebProject.Intrerface;
using MyWebProject.Models;
using MyWebProject.ViewModel;

namespace MyWebProject.services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public ShoppingCartService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        // Retrieves the current shopping cart from session or initializes a new one if not present
        public ShoppingCart GetCart()
        {
            return _httpContextAccessor.HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
        }

        // Adds a specified quantity of a product to the shopping cart
        public void AddToCart(int productId, int quantity)
        {
            var product = _context.AllProducts.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                var cart = GetCart();
                var item = cart.Items.FirstOrDefault(i => i.Product.ProductId == productId);
                if (item != null)
                {
                    item.Quantity += quantity; // Update quantity if item already exists in cart
                }
                else
                {
                    cart.Items.Add(new CartItem { Product = product, Quantity = 1 }); // Add new item to cart
                }
                _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", cart); // Save cart to session
            }
        }

        // Removes a product from the shopping cart
        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.Product.ProductId == productId);
            if (item != null)
            {
                cart.Items.Remove(item); // Remove item from cart
                _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", cart); // Save cart to session
            }
        }

        // Updates the quantity of a product in the shopping cart
        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.Product.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity; // Update quantity of the item
                _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", cart); // Save cart to session
            }
        }

        // Retrieves the total number of items in the shopping cart
        public int GetCartItemCount()
        {
            var cart = GetCart();
            return cart.Items.Sum(i => i.Quantity); // Calculate total quantity of all items in the cart
        }

        // Clears the shopping cart by removing it from session
        public void ClearCart()
        {
            _httpContextAccessor.HttpContext.Session.Remove("Cart"); // Remove cart from session
        }
    }

}

