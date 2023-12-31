using AutoMapper;
using Tangy_DataAccess;
using Tangy_DataAccess.ViewModel;
using Tangy_Models;

namespace Tangy_Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ReverseMap()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Product, ProductDto>()
                .ReverseMap();

            CreateMap<ProductPrice, ProductPriceDto>()
                .ReverseMap();

            CreateMap<OrderHeader, OrderHeaderDto>()
                .ReverseMap();

            CreateMap<OrderDetail, OrderDetailDto>()
                .ReverseMap();

            CreateMap<Order, OrderDto>()
                .ReverseMap();
        }
    }
}
