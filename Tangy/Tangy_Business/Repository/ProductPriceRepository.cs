using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ProductPriceRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductPriceDto> CreateAsync(ProductPriceDto objDto)
        {
            var obj = _mapper.Map<ProductPriceDto, ProductPrice>(objDto);
            // obj.CreatedDate = DateTime.Now;

            var addedObj = await _db.ProductPrices.AddAsync(obj);
            await _db.SaveChangesAsync();

            return _mapper.Map<ProductPrice, ProductPriceDto>(addedObj.Entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var obj = await _db.ProductPrices.FindAsync(id);

            if (obj is null)
            {
                return 0;
            }

            _db.ProductPrices.Remove(obj);
            return await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductPriceDto>> GetAllAsync(int? productId)
        {
            if (productId is not null && productId > 0)
            {
                return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDto>>(await _db.ProductPrices
                                       .Where(x => x.ProductId == productId)
                                                          .ToListAsync());
            }

            return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDto>>(await _db.ProductPrices
                .ToListAsync());
        }

        public async Task<ProductPriceDto> GetAsync(int id)
        {
            var obj = await _db.ProductPrices.FindAsync(id);

            if (obj is null)
            {
                return new ProductPriceDto();
            }

            return _mapper.Map<ProductPrice, ProductPriceDto>(obj);
        }

        public async Task<ProductPriceDto> UpdateAsync(ProductPriceDto objDto)
        {
            var objFromDb = await _db.ProductPrices.FindAsync(objDto.Id);

            if (objFromDb is null)
            {
                return objDto;
            }

            objFromDb.Price = objDto.Price;
            objFromDb.ProductId = objDto.ProductId;
            objFromDb.Size = objDto.Size;


            _db.ProductPrices.Update(objFromDb);
            await _db.SaveChangesAsync();

            return _mapper.Map<ProductPrice, ProductPriceDto>(objFromDb);
        }
    }
}
