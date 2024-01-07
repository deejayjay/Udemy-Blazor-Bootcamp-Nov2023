using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
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

        [HttpPost]
        [ActionName("create")]
        public async Task<IActionResult> Create([FromBody] StripePaymentDto paymentDto)
        {
            paymentDto.Order.OrderHeader.OrderDate = DateTime.Now;
            var result = await _orderRepository.CreateAsync(paymentDto.Order);
            return Ok(result);
        }

        [HttpPost]
        [ActionName("paymentsuccessful")]
        public async Task<IActionResult> PaymentSuccessful([FromBody] OrderHeaderDto orderHeaderDto)
        {
            var service = new SessionService();
            var sessionDetails = await service.GetAsync(orderHeaderDto.SessionId);

            if (sessionDetails is null || !(sessionDetails.PaymentStatus == "paid"))
            {
                return BadRequest();
            }

            var result = await _orderRepository.MarkPaymentSuccessfulAsync(orderHeaderDto.Id);

            if (result is null)
            {
                return BadRequest(new ErrorModelDto
                {
                    ErrorMessage = "Unable to mark payment as successful."
                });
            }
            return Ok(result);
        }
    }
}
