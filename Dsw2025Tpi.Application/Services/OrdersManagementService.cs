using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Validation;
using Dsw2025Tpi.Data.Repositories;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
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
            var order = await _repository.GetById<Order>(id, nameof(Order));
            return order != null ?
                new OrderModel.ResponseOrderModel(order.Id, order.Date, order.ShippingAddress, order.BillingAddress, order.Notes, order.CustomerId) :
                null;
        }

        // Solucionar aqui y ver si usamos filtro o un GetAll
        public async Task<IEnumerable<OrderModel.ResponseOrderModel>?> GetAllOrders()
        {
            return (await _repository
                .GetAll<Order>())?
                .Select(o => new OrderModel.ResponseOrderModel(o.Id, o.Date, o.ShippingAddress, o.BillingAddress, o.Notes,
                o.CustomerId));
        }

        // Hay que ver como podemos validar la fecha o si hace falta, ademas de como verificar que haya suficiente stock al momento de crear la orden y restar la cantidad de los  producto solicitado al stock de los prodcutos

        public async Task<OrderModel.ResponseOrderModel> AddOrder(OrderModel.RequestOrderModel request)
        {

            var exist = await _repository.First<Order>(o => o.Date == request.Date);
            var order = new Order(request.Date, request.ShippingAddress, request.BillingAddress, request.Notes, request.CustomerId);
            OrderValidator.Validate(request);

            foreach (var item in request.Items)
            {
                var prod = await _repository.GetById<Product>(item.ProductId)
                   ?? throw new InvalidOperationException($"Producto no encontrado: {item.ProductId}");
                var orderItem = order.AddItem(prod, item.Quantity);

            }

            await _repository.Add(order);

            return new OrderModel.ResponseOrderModel(order.Id, order.Date, order.ShippingAddress, order.BillingAddress, order.Notes, order.CustomerId);
        }
    }
}
