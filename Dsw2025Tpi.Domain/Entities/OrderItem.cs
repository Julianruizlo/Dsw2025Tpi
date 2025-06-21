using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class OrderItem: EntityBase
{
    public OrderItem( int quantity, decimal unitPrice, Guid orderId, Guid productId)
    {
        ProductId = productId;
        OrderId = orderId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public Guid OrderId { get; set; }
    public Order? Order { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
}
