using BusinessEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Services.Products
{
    public class ProductService
    {
        private static readonly List<Product> _products = new List<Product>();

        public IEnumerable<Product> GetAll(string name = null)
        {
            return string.IsNullOrEmpty(name)
                ? _products
                : _products.Where(p => p.Name.Contains(name));
        }

        public Product GetById(Guid id) => _products.FirstOrDefault(p => p.Id == id);

        public void Add(Product product) => _products.Add(product);

        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index >= 0) _products[index] = product;
        }

        public void Delete(Guid id)
        {
            var item = _products.FirstOrDefault(p => p.Id == id);
            if (item != null) _products.Remove(item);
        }
    }
}
