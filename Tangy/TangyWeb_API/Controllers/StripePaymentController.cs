using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Tangy_Models;

namespace TangyWeb_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StripePaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public StripePaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize]
        [ActionName("Create")]
        public async Task<IActionResult> Create([FromBody] StripePaymentDto paymentDto)
        {
            try
            {
                var domain = _configuration.GetValue<string>("Tangy_Client_Url");

                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    PaymentMethodTypes = new List<string> { "card" },
                    SuccessUrl = $"{domain}/{paymentDto.SuccessUrl}",
                    CancelUrl = $"{domain}/{paymentDto.CancelUrl}"
                };

                foreach (var item in paymentDto.Order.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }
                        },
                        Quantity = item.Count
                    };

                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                return Ok(new SuccessModelDto
                {
                    Data = session.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModelDto
                {
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
