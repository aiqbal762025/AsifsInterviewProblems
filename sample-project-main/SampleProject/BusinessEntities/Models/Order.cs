using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string OrderDate { get; set; } = DateTime.UtcNow.ToString();
    }
}
