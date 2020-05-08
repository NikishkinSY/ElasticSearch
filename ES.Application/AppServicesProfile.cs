using AutoMapper;
using ES.Application.ElasticSearch.Entities;
using ES.Domain.Entities;

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
