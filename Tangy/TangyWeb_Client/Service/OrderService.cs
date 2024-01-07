using Newtonsoft.Json;
using System.Text;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _client;
        
        public OrderService(HttpClient client)
        {
            _client = client;                   
        }

        public async Task<OrderDto> CreateAsync(StripePaymentDto paymentDto)
        {
            var content = JsonConvert.SerializeObject(paymentDto);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/order/create", bodyContent);
            var responseResult = await response.Content.ReadAsStringAsync();

            if(!response.IsSuccessStatusCode)
            {
                return new OrderDto();
            }

            var result = JsonConvert.DeserializeObject<OrderDto>(responseResult);
            return result ?? new OrderDto();
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync(string? userId = null)
        {
            var response = await _client.GetAsync("/api/order/getall");

            if (!response.IsSuccessStatusCode)
            {
                return new List<OrderDto>();
            }

            var content = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(content);            

            return orders ?? new List<OrderDto>();
        }

        public async Task<OrderDto> GetAsync(int orderHeaderId)
        {
            var response = await _client.GetAsync($"/api/order/get/{orderHeaderId}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content);

                throw new Exception(errorModel?.ErrorMessage);
            }


            var order = JsonConvert.DeserializeObject<OrderDto>(content);            

            return order ?? new OrderDto();
        }

        public async Task<OrderHeaderDto> MarkPaymentSuccessful(OrderHeaderDto orderHeader)
        {
            var content = JsonConvert.SerializeObject(orderHeader);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/order/paymentsuccessful", bodyContent);
            var responseResult = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(responseResult);
                throw new Exception(errorModel?.ErrorMessage);
            }

            var result = JsonConvert.DeserializeObject<OrderHeaderDto>(responseResult);
            return result ?? new OrderHeaderDto();
        }
    }
}
