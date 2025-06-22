using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services
{
    public class OrdersManagementService
    {
        private readonly IRepository _repository;
        

        public OrdersManagementService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<OrderModel.Response?> GetOrderById(Guid id)
        {
            var order = await _repository.GetById<Order>(id, nameof(Order));
            return order != null ?
                new OrderModel.Response(order.Id, order.Date, order.ShippingAddres, order.BillingAddress, order.Notes, order.CustomerId) :
                null;
        }

        // Solucionar aqui y ver si usamos filtro o un GetAll
        public async Task<IEnumerable<OrderModel.Response>?> GetOrders()
        {
            return (await _repository
                .GetFiltered<Order>(o => o.Date == DateTime.Today, nameof(Order)))?
                .Select(o => new OrderModel.Response(o.Id, o.Date, o.ShippingAddres, o.BillingAddress, o.Notes,
                o.CustomerId));
        }

        // Hay que ver como podemos validar la fecha o si hace falta, ademas de como verificar que haya suficiente stock al momento de crear la orden y restar la cantidad de los  producto solicitado al stock de los prodcutos

        public async Task<OrderModel.Response> AddOrder(OrderModel.Request request)
        {
            if  (
                   request.CustomerId == Guid.Empty
                )
            {

                throw new ArgumentException("Valores para el producto no válidos");
            }

            var exist = await _repository.First<Order>(o => o.Date == request.Date);
            var order = new Order(request.Date, request.ShippingAddres, request.BillingAddress, request.Notes, request.CustomerId);
            await _repository.Add(order);

            return new OrderModel.Response(order.Id, order.Date, order.ShippingAddres, order.BillingAddress, order.Notes, order.CustomerId);
        }
    }
}

/*
if (request.CustomerId == Guid.Empty)
            {
                throw new ArgumentException("Valores para el producto no válidos");
            }

            // Verificar stock de cada producto en la orden
            foreach (var item in request.Items) // Asumiendo que Items es la lista de productos y cantidades
            {
                var product = await _repository.GetById<Product>(item.ProductId, nameof(Product));
                if (product == null)
                    throw new ArgumentException($"Producto con ID {item.ProductId} no encontrado");

                if (product.StockQuantity < item.Quantity)
                    throw new InvalidOperationException($"No hay suficiente stock para el producto {product.Name}");
            }

            var order = new Order(request.Date, request.ShippingAddres, request.BillingAddress, request.Notes, request.CustomerId);
            await _repository.Add(order);
            return new OrderModel.Response(order.Id, order.Date, order.ShippingAddres, order.BillingAddress, order.Notes, order.CustomerId);
*/