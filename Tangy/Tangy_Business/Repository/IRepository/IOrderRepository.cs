using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{
    public interface IOrderRepository
    {
        Task<OrderDto> GetAsync(int id);
        Task<IEnumerable<OrderDto>> GetAllAsync(string? userId = null, string? status = null);
        Task<OrderDto> CreateAsync(OrderDto objDto);
        Task<int> DeleteAsync(int id);

        Task<OrderHeaderDto> UpdateHeaderAsync(OrderHeaderDto objDto);
        Task<OrderHeaderDto> MarkPaymentSuccessfulAsync(int id, string paymentIntentId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<OrderHeaderDto> CancelOrder(int orderId);
    }
}
