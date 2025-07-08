using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.Application.Interfaces
{
    public interface IOrdersManagementService
    {
        Task<OrderModel.ResponseOrderModel> GetOrderById(Guid id);

        Task<IEnumerable<OrderModel.ResponseOrderModel>?> GetAllOrders();

        Task<OrderModel.ResponseOrderModel> AddOrder(OrderModel.RequestOrderModel request);

        Task<OrderModel.ResponseOrderModel> PutOrder(Guid id,OrderModel.RequestOrderModel request);
    }
}
