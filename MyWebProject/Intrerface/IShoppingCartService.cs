using MyWebProject.ViewModel;

namespace MyWebProject.Intrerface
{
    public interface IShoppingCartService
    {
        // Retrieves the current shopping cart
        ShoppingCart GetCart();

        // Adds a specified quantity of a product to the shopping cart
        void AddToCart(int productId, int quantity);

        // Removes a product from the shopping cart
        void RemoveFromCart(int productId);

        // Updates the quantity of a product in the shopping cart
        void UpdateQuantity(int productId, int quantity);

        // Retrieves the total number of items in the shopping cart
        int GetCartItemCount();

        // Clears the shopping cart, removing all items
        void ClearCart();
    }


}
