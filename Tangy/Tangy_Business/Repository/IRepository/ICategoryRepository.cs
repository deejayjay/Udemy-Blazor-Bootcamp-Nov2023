using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<CategoryDto> CreateAsync(CategoryDto objDto);
        Task<CategoryDto> UpdateAsync(CategoryDto objDto);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetAsync(int id);
    }
}
