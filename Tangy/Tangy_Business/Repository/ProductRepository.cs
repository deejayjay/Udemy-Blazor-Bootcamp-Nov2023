using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateAsync(ProductDto objDto)
        {
            var obj = _mapper.Map<ProductDto, Product>(objDto);

            var addedObj = await _db.Products.AddAsync(obj);
            await _db.SaveChangesAsync();

            return _mapper.Map<Product, ProductDto>(addedObj.Entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var obj = await _db.Products.FindAsync(id);

            if (obj is null)
            {
                return 0;
            }

            _db.Products.Remove(obj);
            return await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return _mapper
                .Map<IEnumerable<Product>, IEnumerable<ProductDto>>(await _db.Products
                                                                                .Include(p => p.Category)
                                                                                .ToListAsync());
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            var obj = await _db.Products
                                .Include(p => p.Category)
                                .FirstOrDefaultAsync(p => p.Id == id);

            if (obj is null)
            {
                return new ProductDto();
            }

            return _mapper.Map<Product, ProductDto>(obj);
        }

        public async Task<ProductDto> UpdateAsync(ProductDto objDto)
        {
            var objFromDb = await _db.Products.FindAsync(objDto.Id);

            if (objFromDb is null)
            {
                return objDto;
            }

            objFromDb.Name = objDto.Name;
            objFromDb.Description = objDto.Description;
            objFromDb.IsShopFavorite = objDto.IsShopFavorite;
            objFromDb.IsCustomerFavorite = objDto.IsCustomerFavorite;
            objFromDb.Color = objDto.Color;
            objFromDb.ImageUrl = objDto.ImageUrl;
            objFromDb.CategoryId = objDto.CategoryId;

            _db.Products.Update(objFromDb);
            await _db.SaveChangesAsync();

            return _mapper.Map<Product, ProductDto>(objFromDb);
        }
    }
}
