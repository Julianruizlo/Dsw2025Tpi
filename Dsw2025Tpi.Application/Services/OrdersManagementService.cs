using Azure.Core;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Validation;
using Dsw2025Tpi.Data.Repositories;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services
{
    public class OrdersManagementService : IOrdersManagementService
    {
        private readonly IRepository _repository;
        

        public OrdersManagementService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<OrderModel.ResponseOrderModel?> GetOrderById(Guid id)
        {
            var order = await _repository.GetById<Order>(id, nameof(Order.OrderItems), "OrderItems.Product");
            if(order == null) throw new InvalidOperationException($"Order not found");

            var responseItems = order.OrderItems.Select(i => new OrderItemModel.ResponseOrderItemModel(
                    i.Id,
                    i.Quantity,
                    i.UnitPrice,
                    i.OrderId,
                    i.ProductId
                )).ToList();

            return order != null ?
                new OrderModel.ResponseOrderModel(order.Id, order.Date, order.ShippingAddress, order.BillingAddress, order.Notes, order.CustomerId, order.Status, responseItems) :
                null;
        }

        public async Task<IEnumerable<OrderModel.ResponseOrderModel>?> GetAllOrders(OrderModel.SearchOrder request)
        {
            OrderStatus? status = null;
            if (!string.IsNullOrWhiteSpace(request.Status))
                status = Enum.Parse<OrderStatus>(request.Status.ToUpper(), true);
            var orders = await _repository
            .GetFiltered<Order>(
                o =>
                    o.Status != OrderStatus.CANCELLED
                    && (o.CustomerId == request.CustomerId||request.CustomerId.ToString().IsNullOrEmpty())
                    && (!status.HasValue||o.Status == status.Value),
                    
                 include: new[] { "OrderItems" }
            );
            return orders.Select(order => new OrderModel.ResponseOrderModel(order.Id, order.Date, order.ShippingAddress, order.BillingAddress, order.Notes, order.CustomerId, order.Status, order.OrderItems.Select(i => new OrderItemModel.ResponseOrderItemModel(i.Id,
                i.Quantity, i.UnitPrice, i.OrderId, i.ProductId)).ToList()));
        }

        public async Task<OrderModel.ResponseOrderModel> AddOrder(OrderModel.RequestOrderModel request)
        {
            OrderValidator.Validate(request);

            if (request.Items == null || !request.Items.Any())
                throw new ArgumentException("The order must have at least one item.");

            var order = new Order(
                request.Date,
                request.ShippingAddress,
                request.BillingAddress,
                request.Notes,
                request.CustomerId
            );

            await _repository.Add(order);

            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var item in request.Items)
            {
                var product = await _repository.GetById<Product>(item.ProductId)
                    ?? throw new InvalidOperationException($"Product not found: {item.ProductId}");

                if (product.StockQuantity < item.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for product: {product.Name}");

                product.StockQuantity -= item.Quantity;
                await _repository.Update(product);

                var orderItem = new OrderItem(
                    item.Quantity,
                    product.CurrentUnitPrice,
                    order.Id,
                    product.Id
                );
                orderItems.Add(orderItem);
                totalAmount += product.CurrentUnitPrice * item.Quantity;
            }

            order.OrderItems = orderItems;
            await _repository.Update(order);

            var responseItems = orderItems.Select(oi => new OrderItemModel.ResponseOrderItemModel(
                oi.Id,
                oi.Quantity,
                oi.UnitPrice,
                oi.OrderId,
                oi.ProductId
            )).ToList();

            return new OrderModel.ResponseOrderModel(
                order.Id,
                order.Date,
                order.ShippingAddress,
                order.BillingAddress,
                order.Notes,
                order.CustomerId,
                order.Status,
                responseItems
            );
        }

        public async Task<OrderModel.ResponseOrderModel> PutOrder(Guid id, OrderModel.RequestOrderModel request)
        {
            OrderValidator.Validate(request);

            if (!Enum.IsDefined(typeof(OrderStatus), request.Status))
            {
                throw new ArgumentOutOfRangeException("The state entered is not valid.");
            }

            var exist = await _repository.GetById<Order>(id);
            if (exist == null)
                throw new KeyNotFoundException($"Order with ID: {id} not found");
            exist.Status = request.Status;

            await _repository.Update(exist);

            var responseItems = exist.OrderItems.Select(oi => new OrderItemModel.ResponseOrderItemModel(
            oi.Id,
            oi.Quantity,
            oi.UnitPrice,
            oi.OrderId,
            oi.ProductId
            )).ToList();


            return new OrderModel.ResponseOrderModel
           (
                exist.Id,
                exist.Date,
                exist.ShippingAddress,
                exist.BillingAddress,
                exist.Notes,
                exist.CustomerId,
                exist.Status,
                responseItems
            );
        }
    }
}

