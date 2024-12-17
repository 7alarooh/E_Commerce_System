using AutoMapper;
using E_CommerceSystem.Models;
using E_CommerceSystem.Models.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_CommerceSystem.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InputProductDTO, Product>();
            CreateMap<Product, OutputProductDTO>();

            CreateMap<Order, OutputOrderDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderProducts));

            CreateMap<OrderProducts, OutputOrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<InputOrderDTO, Order>();
        }
    }
}
