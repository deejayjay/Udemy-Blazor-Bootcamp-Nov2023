using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{
    public interface IProductPriceRepository
    {
        Task<ProductPriceDto> CreateAsync(ProductPriceDto objDto);
        Task<ProductPriceDto> UpdateAsync(ProductPriceDto objDto);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<ProductPriceDto>> GetAllAsync(int? productId);
        Task<ProductPriceDto> GetAsync(int id);
    }
}
