using Tangy_Models;

namespace TangyWeb_Client.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetAsync(int productId);
    }
}
