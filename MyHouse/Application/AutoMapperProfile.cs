using AutoMapper;
using Contract.Identity.RoleManager;
using Contract.Identity.UserManager;
using Contract.PartReviewDetails;
using Contract.PartReviews;
using Contract.Parts;
using Domain.Identity.Roles;
using Domain.Identity.Users;
using Domain.PartReviewDetails;
using Domain.PartReviews;
using Domain.Parts;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;

namespace Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUpdateUserDto,User>().ReverseMap();
            CreateMap<User,UserDto>().ReverseMap();
            CreateMap<Role,RoleDto>().ReverseMap();
            CreateMap<RoleDto,CreateUpdateRoleDto>().ReverseMap();
            CreateMap<Role,CreateUpdateRoleDto>().ReverseMap();
            
            CreateMap<Part,CreateUpdatePartDto>().ReverseMap();
            CreateMap<Part,Contract.Parts.PartDto>().ReverseMap();
            CreateMap<CreateUpdatePartDto,Contract.Parts.PartDto>().ReverseMap();

            CreateMap<CreateUpdatePartReviewDto, PartReview>().ReverseMap();
            CreateMap<PartReview, PartReviewDto>().ReverseMap();
            CreateMap<PartReviewDetailDto, PartReviewDetail>().ReverseMap();
            CreateMap<CreateUpdatePartReviewDetailDto, PartReviewDetail>().ReverseMap();

        }
    }
}