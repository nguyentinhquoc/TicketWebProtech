using AutoMapper;
using ProtechGroup.Domain.Entities;
using ProtechGroup.Infrastructure.Entities;

namespace ProtechGroup.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain -> Infrastructure
            CreateMap<SearchInputMod, SearchInput>();
            CreateMap<AirportMod, Airport>();
            CreateMap<SearchWSHistoryMod, SearchWSHistory>();
            CreateMap<NewsMod, News>();
            CreateMap<HotelMod, Hotel>();

            // Infrastructure -> Domain
            CreateMap<SearchInput, SearchInputMod>();
            CreateMap<Airport, AirportMod>();
            CreateMap<SearchWSHistory, SearchWSHistoryMod>();
            CreateMap<ServiceFee, ServiceFeeMod>();
            CreateMap<News, NewsMod>();
            CreateMap<Hotel, HotelMod>();
        }
    }
}
