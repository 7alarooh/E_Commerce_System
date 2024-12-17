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
        }
    }
}
