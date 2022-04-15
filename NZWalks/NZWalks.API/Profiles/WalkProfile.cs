using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class WalkProfile : Profile
    {
        public WalkProfile()
        {
            CreateMap<Models.Domain.Walk, Models.DTO.Walk>();
            CreateMap<Models.DTO.UpdateWalkRequest, Models.Domain.Walk>();
            CreateMap<Models.DTO.AddWalkRequest, Models.Domain.Walk>();
            CreateMap<Models.Domain.WalkDifficulty, Models.DTO.WalkDifficulty>().ReverseMap();
        }
    }
}
