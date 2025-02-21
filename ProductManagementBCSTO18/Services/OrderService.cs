using Microsoft.EntityFrameworkCore;
using ProductManagementBCSTO18.Interfaces;
using ProductManagementBCSTO18.Models;
using ProductManagementBCSTO18.Models.Entities;
using ProductManagementBCSTO18.Models.VM.Order;

namespace ProductManagementBCSTO18.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(OrderViewModel model)
        {
            var order = new Order()
            {
                Products = new List<Product>()
            };

            foreach (var productId in model.ProductIds)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
                if (product != null)
                    order.Products.Add(product);
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var orderToDelete = await _context.Orders.FirstOrDefaultAsync(x => x.Id != orderId);

            _context.Orders.Remove(orderToDelete);
            await _context.SaveChangesAsync();
        }

        public Task<List<Order>> GetOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateOrderAsync(int orderId, OrderViewModel model)
        {
            var order = await _context.Orders.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == orderId);

            var products = new List<Product>();

            foreach (var productId in model.ProductIds)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
                if(product != null)
                    products.Add(product);
            }

            order.Products = products;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
