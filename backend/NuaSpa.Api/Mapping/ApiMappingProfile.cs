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
    }
}
