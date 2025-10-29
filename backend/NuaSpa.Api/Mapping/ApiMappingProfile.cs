using AutoMapper;
using NuaSpa.Api.Models;
using NuaSpa.Core.Entities;

namespace NuaSpa.Api.Mapping;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<ServiceCategory, ServiceCategoryDto>();
        CreateMap<CreateServiceCategoryDto, ServiceCategory>();
        CreateMap<UpdateServiceCategoryDto, ServiceCategory>();

        CreateMap<Service, ServiceDto>();
        CreateMap<CreateServiceDto, Service>();
        CreateMap<UpdateServiceDto, Service>();

        CreateMap<Staff, StaffDto>()
    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

    }
}
