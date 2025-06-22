using Dsw2025Ej15.Application.Exceptions;
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

        // Hay que ver como podemos validar la fecha o si hace falta

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
