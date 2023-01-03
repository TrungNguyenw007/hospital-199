using AutoMapper;
using Contract.Identity.RoleManager;
using Contract.Identity.UserManager;
using Contract.PartReviewDetails;
using Contract.PartReviews;
using Contract.Parts;
using Domain.Parts;
using WebClient.Pages.Admin;
using WebClient.Pages.Client;

namespace WebClient
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RoleDto, CreateUpdateRoleDto>().ReverseMap();
            CreateMap<UserDto, CreateUpdateUserDto>().ReverseMap();
            CreateMap<UserWithNavigationDto, CreateUpdateUserWithNavDto>().ReverseMap();

            CreateMap<CreateUpdatePartDto,Contract.Parts.PartDto>().ReverseMap();
            CreateMap<PartReviewDetailDto, CreateUpdatePartReviewDetailDto>().ReverseMap();

            CreateMap<PartReviewDto, CreateUpdatePartReviewDto>().ReverseMap();
            CreateMap<PartDto, PartReview.ReviewWithNavPropertiesModel>().ForMember(
                x => x.PartId,
                y => y.MapFrom(x => x.Id)
            );

            CreateMap<PartReview.ReviewWithNavPropertiesModel, CreateUpdatePartReviewDto>();













            // CreateMap<A, C>().ReverseMap();
            //
            // CreateMap<B, E>().ForMember(dest=>dest.deptrai1,
            //     opt=>opt.MapFrom(source => source.deptrai)).ReverseMap();


        }
    }
}