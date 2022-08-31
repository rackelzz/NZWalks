using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class RegionsProfile : Profile
    {

        public RegionsProfile()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>();
                //.ForMemeber(dest => dest.Id, options => options.MapFrom(src => src.RegionId)); dont need here bc names are same
                //.ReverseMap();
        }


    }   
}
