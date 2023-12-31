using Microsoft.AspNetCore.Mvc;
using Tangy_Business.Repository.IRepository;
using Tangy_Models;

namespace TangyWeb_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var objList = await _orderRepository.GetAllAsync();
            return Ok(objList);
        }

        // GET: api/Order/{id}
        [HttpGet("{orderHeaderId}")]
        public async Task<IActionResult> Get(int? orderHeaderId)
        {
            if (orderHeaderId is null || orderHeaderId == 0) 
            {
                return BadRequest(new ErrorModelDto() 
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid Order Id."
                });
            }

            var order = await _orderRepository.GetAsync(orderHeaderId.Value);
            if (order is null)
            {
                return NotFound(new ErrorModelDto()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"Could not find a order with an ID of {orderHeaderId.Value}"
                });
            }

            return Ok(order);
        }
    }
}
