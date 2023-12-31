using AutoMapper;
using Tangy_Business.Repository.IRepository;
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
            catch
            {
                throw;
            }            
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDto>> GetAllAsync(string? userId = null, string? status = null)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderHeaderDto> MarkPaymentSuccessfulAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderHeaderDto> UpdateHeaderAsync(OrderHeaderDto objDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
