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
    public decimal Subtotal => Quantity * UnitPrice;

    public Guid OrderId { get; set; }
    public Order? Order { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

}


// No store type was specified for the decimal property 'UnitPrice' on entity type 'OrderItem'.
// This will cause values to be silently truncated if they do not fit in the default precision and scale.
// Explicitly specify the SQL server column type that can accommodate all the values in 'OnModelCreating' using 'HasColumnType',
// specify precision and scale using 'HasPrecision', or configure a value converter using 'HasConversion'.

