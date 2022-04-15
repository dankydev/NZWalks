using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>();
            CreateMap<Models.DTO.UpdateRegionRequest, Models.Domain.Region>();
            CreateMap<Models.DTO.AddRegionRequest, Models.Domain.Region>();
        }
    }
}
