using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository.IRepository;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_DataAccess.ViewModel;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public OrderRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateAsync(OrderDto objDto)
        {
            try
            {
                var obj = _mapper.Map<OrderDto, Order>(objDto);

                // Create Order Header
                await _db.OrderHeaders.AddAsync(obj.OrderHeader);
                await _db.SaveChangesAsync();

                // Create Order Details
                foreach (var details in obj.OrderDetails)
                {
                    details.OrderHeaderId = obj.OrderHeader.Id;
                    await _db.OrderDetails.AddAsync(details);
                }
                // Instead of doing await _db.OrderDetails.AddAsync(details) in foreach loop, we can do this:
                // await _db.OrderDetails.AddRangeAsync(obj.OrderDetails);
                await _db.SaveChangesAsync();

                return new OrderDto()
                {
                    OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDto>(obj.OrderHeader),
                    OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDto>>(obj.OrderDetails)
                                    .ToList()
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                var objHeader = await _db.OrderHeaders.FindAsync(id);

                if (objHeader is null)
                {
                    return 0;
                }

                var objDetails = _db.OrderDetails.Where(o => o.OrderHeaderId == id);

                // Removes the list of OrderDetails
                _db.OrderDetails.RemoveRange(objDetails);

                // Remvoes the OrderHeader
                _db.OrderHeaders.Remove(objHeader);

                return await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync(string? userId = null, string? status = null)
        {
            var ordersFromDb = new List<Order>();

            IEnumerable<OrderHeader> orderHeaderList = await _db.OrderHeaders.ToListAsync();
            IEnumerable<OrderDetail> orderDetailList = await _db.OrderDetails.ToListAsync();

            foreach (var header in orderHeaderList)
            {
                Order order = new()
                {
                    OrderHeader = header,
                    OrderDetails = orderDetailList.Where(o => o.OrderHeaderId == header.Id),
                };

                ordersFromDb.Add(order);
            }

            // TODO: Filter by userId and status

            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(ordersFromDb);
        }

        public async Task<OrderDto> GetAsync(int id)
        {
            Order order = new()
            { 
                OrderHeader = await _db.OrderHeaders.FindAsync(id) ?? new OrderHeader(),
                OrderDetails = await _db.OrderDetails.Where(o => o.OrderHeaderId == id).ToListAsync()
            };

            if (order == null)
            {
                return new OrderDto();
            }

            return _mapper.Map<Order, OrderDto>(order);
        }

        public async Task<OrderHeaderDto> MarkPaymentSuccessfulAsync(int id)
        {
            var data = await _db.OrderHeaders.FindAsync(id);

            if(data is null)
            {
                return new OrderHeaderDto();
            }

            if(data.Status == Sd.Status_Pending)
            {
                data.Status = Sd.Status_Confirmed;
                await _db.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDto>(data);
            }
            
            return new OrderHeaderDto();
        }

        public async Task<OrderHeaderDto> UpdateHeaderAsync(OrderHeaderDto objDto)
        {
            if (objDto == null)
            {
                return new OrderHeaderDto();
            }

            var orderHeader = _mapper.Map<OrderHeaderDto, OrderHeader>(objDto);
            _db.OrderHeaders.Update(orderHeader);
            await _db.SaveChangesAsync();
            return _mapper.Map<OrderHeader, OrderHeaderDto>(orderHeader);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var data = await _db.OrderHeaders.FindAsync(orderId);

            if (data is null)
            {
                return false;
            }

            data.Status = status;

            if (status == Sd.Status_Shipped)
            {
                data.ShippingDate = DateTime.Now;
            }

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
