using Microsoft.EntityFrameworkCore;
using MyWebProject.Date;
using MyWebProject.Intrerface;
using MyWebProject.Models;
using MyWebProject.ViewModel;

namespace MyWebProject.services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Retrieves orders filtered by optional start and end dates
        public IEnumerable<Order> GetOrders(DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Order> query = _context.Orders;

            if (startDate.HasValue && endDate.HasValue)
            {
                // Filter orders based on order date falling within the specified date range
                query = query.Where(o => o.OrderDate.Date >= startDate.Value.Date && o.OrderDate.Date <= endDate.Value.Date);
            }

            return query.ToList();
        }

        // Retrieves details of a specific order including associated items and products
        public Order GetOrderDetails(int id)
        {
            return _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefault(o => o.OrderId == id);
        }

        // Places a new order based on the current shopping cart stored in session
        public void PlaceOrder()
        {
            var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Count > 0)
            {
                // Create a new order entity
                var order = new Order
                {
                    UserId = _httpContextAccessor.HttpContext.User.Identity.Name, // Set the user ID from HttpContext
                    OrderDate = DateTime.Now, // Set the current date/time as the order date
                    TotalPrice = cart.TotalPrice,
                    Items = new List<CartItem>()
                };

                // Populate order items from the shopping cart
                foreach (var item in cart.Items)
                {
                    var product = _context.AllProducts.FirstOrDefault(p => p.ProductId == item.Product.ProductId);
                    if (product != null)
                    {
                        order.Items.Add(new CartItem { Product = product, Quantity = item.Quantity });
                    }
                }

                // Save the order to the database
                _context.Orders.Add(order);
                _context.SaveChanges();

                // Clear the shopping cart stored in session after placing the order
                _httpContextAccessor.HttpContext.Session.Remove("Cart");
            }
        }

        // Deletes an order and its associated items from the database
        public void DeleteOrder(int orderId)
        {
            var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                // Remove the order and its associated items from the database
                _context.Orders.RemoveRange(order); // Remove related items first
                _context.Orders.Remove(order); // Remove the order itself
                _context.SaveChanges();
            }
        }
    }

}
