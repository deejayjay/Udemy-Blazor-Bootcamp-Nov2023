using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<ProductDto> CreateAsync(ProductDto objDto);
        Task<ProductDto> UpdateAsync(ProductDto objDto);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetAsync(int id);
    }
}
