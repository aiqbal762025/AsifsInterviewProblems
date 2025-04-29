using BusinessEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Services.Orders
{
    public class OrderService
    {
        private static readonly List<Order> _orders = new List<Order>();

        public IEnumerable<Order> GetAll(Guid? productId = null)
        {
            return productId.HasValue
                ? _orders.Where(o => o.ProductId.Equals(productId.ToString()))
                : _orders;
        }

        public Order GetById(Guid id) => _orders.FirstOrDefault(o => o.Id == id);

        public void Add(Order order) => _orders.Add(order);

        public void Update(Order order)
        {
            var index = _orders.FindIndex(o => o.Id == order.Id);
            if (index >= 0) _orders[index] = order;
        }

        public void Delete(Guid id)
        {
            var item = _orders.FirstOrDefault(o => o.Id == id);
            if (item != null) _orders.Remove(item);
        }
    }
}
