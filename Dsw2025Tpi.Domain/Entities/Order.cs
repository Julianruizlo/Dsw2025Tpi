﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

public class Order: EntityBase
{
    public Order(DateTime date, string? shippingAddress, string? billingAddress, string? notes, Guid customerId)
    {
        CustomerId = customerId;
        Date = date;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Notes = notes;
        Status = OrderStatus.PENDING;
        OrderItems = [];
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        Status = newStatus;
    }

    //Copilot hizo el codigo para comprobar y restar cuando se solicite la cantidad de un producto del stock del mismo
    public OrderItem AddItem(Product product, int quantity)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(quantity));

        if (!product.IsActive)
            throw new InvalidOperationException("El producto no está activo.");

        if (product.StockQuantity < quantity)
            throw new InvalidOperationException("No hay stock suficiente para este producto.");

        // Descontar stock
        product.StockQuantity -= quantity;

        var item = new OrderItem(quantity, product.CurrentUnitPrice, this.Id, product.Id);
        OrderItems.Add(item);

        return item;
    }

    public DateTime Date { get; set; }
    public string? ShippingAddress { get; set; }
    public string? BillingAddress { get; set; }
    public string? Notes { get; set; }
    public decimal TotalAmount => OrderItems.Sum(p => p.Subtotal);


    public OrderStatus Status { get; private set; }
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } 

}
