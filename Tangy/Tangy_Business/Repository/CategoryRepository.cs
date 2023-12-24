using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CategoryDto> CreateAsync(CategoryDto objDto)
        {
            var obj = _mapper.Map<CategoryDto, Category>(objDto);
            // obj.CreatedDate = DateTime.Now;

            var addedObj = await _db.Categories.AddAsync(obj);
            await _db.SaveChangesAsync();
            
            return _mapper.Map<Category, CategoryDto>(addedObj.Entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var obj = await _db.Categories.FindAsync(id);

            if (obj is null)
            {
                return 0;
            }

            _db.Categories.Remove(obj);
            return await _db.SaveChangesAsync();
        }

        public async Task<CategoryDto> GetAsync(int id)
        {
            var obj = await _db.Categories.FindAsync(id);

            if (obj is null)
            {
                return new CategoryDto();
            }

            return _mapper.Map<Category, CategoryDto>(obj);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(await _db.Categories.ToListAsync());
        }

        public async Task<CategoryDto> UpdateAsync(CategoryDto objDto)
        {
            var objFromDb = await _db.Categories.FindAsync(objDto.Id);

            if (objFromDb is null)
            {
                return objDto;
            }

            objFromDb.Name = objDto.Name;
            
            _db.Categories.Update(objFromDb);
            await _db.SaveChangesAsync();

            return _mapper.Map<Category, CategoryDto>(objFromDb);
        }
    }
}
