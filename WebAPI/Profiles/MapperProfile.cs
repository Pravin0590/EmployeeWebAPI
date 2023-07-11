using AutoMapper;
using Models;
using WebAPI.Models;

namespace WebAPI.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}