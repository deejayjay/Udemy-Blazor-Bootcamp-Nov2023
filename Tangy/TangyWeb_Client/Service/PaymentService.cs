using Newtonsoft.Json;
using System.Text;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;        

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;            
        }

        public async Task<SuccessModelDto> Checkout(StripePaymentDto model)
        {
            try
            {
                var content = JsonConvert.SerializeObject(model);
                var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/stripepayment/create", bodyContent);
                var responseResult = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(responseResult);
                    throw new Exception(errorModel?.ErrorMessage);
                }

                var result = JsonConvert.DeserializeObject<SuccessModelDto>(responseResult);
                return result ?? new SuccessModelDto();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
