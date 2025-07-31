using Dsw2025Tpi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos;

public record OrderModel
{
    public record RequestOrderModel( DateTime Date, string? ShippingAddress, string? BillingAddress, string? Notes, Guid CustomerId, List<OrderItemModel.RequestOrderItemModel> Items, OrderStatus Status);
    public record OrderItemRequest(Guid ProductId, int Quantity);
    public record ResponseOrderModel(Guid Id, DateTime Date, string? ShippingAddress, string? BillingAddress, string? Notes, Guid CustomerId, OrderStatus Status, decimal TotatAmount, List<OrderItemModel.ResponseOrderItemModel> Items);

    public record SearchOrder(Guid? CustomerId, string? Status );
}
