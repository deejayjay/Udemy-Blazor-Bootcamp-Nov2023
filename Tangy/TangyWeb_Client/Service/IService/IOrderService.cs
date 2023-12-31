﻿using Tangy_Models;

namespace TangyWeb_Client.Service.IService
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync(string? userId);
        Task<OrderDto> GetAsync(int orderId);
    }
}
