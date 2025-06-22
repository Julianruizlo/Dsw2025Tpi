using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class Order: EntityBase
{
    public Order(DateTime date, string? shippingAddres, string? billingAddress, string? notes, Guid customerId)
    {
        CustomerId = customerId;
        Date = date;
        ShippingAddres = shippingAddres;
        BillingAddress = billingAddress;
        Notes = notes;
        Status = OrderStatus.PENDING;
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        Status = newStatus;
    }

    public DateTime Date { get; set; }
    public string? ShippingAddres { get; set; }
    public string? BillingAddress { get; set; }
    public string? Notes { get; set; }
   

    //public decimal TotalAmount => _items.Sum(i => i.Subtotal);

    public OrderStatus Status { get; private set; }

    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public ICollection<OrderItem>? OrderItems { get; set; } 

}
