using AutoMapper;
using ES.Domain.Entities;
using ES.Infrastructure.ElasticSearch.Entities;

namespace ES.Application
{
    public class AppServicesProfile : Profile
    {
        public AppServicesProfile()
        {
            CreateMap<Management, ManagementES>().ReverseMap();
            CreateMap<Property, PropertyES>().ReverseMap();
        }
    }
}
