using ProductManagementBCSTO18.Models.Entities;
using ProductManagementBCSTO18.Models.VM.Order;

namespace ProductManagementBCSTO18.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(OrderViewModel model);
        Task<List<Order>> GetOrdersAsync();
        Task UpdateOrderAsync(int orderId, OrderViewModel model);
        Task DeleteOrderAsync(int orderId);
    }
}
