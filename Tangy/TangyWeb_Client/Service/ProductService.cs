using Newtonsoft.Json;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;
        private IConfiguration _configuration;
        private string _baseServerUrl;
        
        public ProductService(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
            _baseServerUrl = _configuration.GetValue<string>("BaseServerUrl");
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var response = await _client.GetAsync("/api/product");

            if (!response.IsSuccessStatusCode)
            {
                return new List<ProductDto>();
            }

            var content = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(content);

            foreach (var product in products!)
            {
                product!.ImageUrl = $"{_baseServerUrl}{product.ImageUrl}";
            }

            return products ?? new List<ProductDto>();
        }

        public async Task<ProductDto> GetAsync(int productId)
        {
            var response = await _client.GetAsync($"/api/product/{productId}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content);
                
                throw new Exception(errorModel?.ErrorMessage);                
            }

            
            var product = JsonConvert.DeserializeObject<ProductDto>(content);
            product!.ImageUrl = $"{_baseServerUrl}{product.ImageUrl}";

            return product ?? new ProductDto();
        }
    }
}
