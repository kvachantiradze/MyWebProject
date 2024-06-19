using MyWebProject.Models;

namespace MyWebProject.Intrerface
{
    public interface IOrderService
    {
        // Retrieves orders within a specified date range
        IEnumerable<Order> GetOrders(DateTime? startDate, DateTime? endDate);

        // Retrieves details of a specific order by its ID
        Order GetOrderDetails(int id);

        // Places a new order
        void PlaceOrder();

        // Deletes an order by its ID
        void DeleteOrder(int orderId);
    }

}
