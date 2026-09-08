using AutoMapper;
using Ctrl_Save.Models;
using Ctrl_Save.Models.DTOs;

namespace Ctrl_Save.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product -> ProductDto
            CreateMap<Product, ProductDto>();

            // ProductCreateDto -> Product
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.Listed, opt => opt.Ignore())
                .ForMember(dest => dest.IsAvailable, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore());

            // Order -> OrderDto
            CreateMap<Order, OrderDto>();

            // OrderItem -> OrderItemDto
            CreateMap<OrderItem, OrderItemDto>();
        }
    }
}
