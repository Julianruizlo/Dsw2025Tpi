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
        Subtotal = quantity * unitPrice;
    }
    public int Quantity 
    { get => Quantity;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("La cantidad debe ser mayor a 0");
            }
        } 
     }
    public decimal UnitPrice 
    { get => UnitPrice;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("El precio unitario debe ser mayor a 0");
            }
        }
     }
    public decimal Subtotal
    {
        get => Subtotal;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("El subtotal debe ser mayor a 0");
            }
        }
    }
    

    public Guid OrderId { get; set; }
    public Order? Order { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

}
