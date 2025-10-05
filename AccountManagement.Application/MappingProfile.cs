using AccountManagment.Contracts.UserContracts;
using AccountManagment.Domain.UserAgg;
using AutoMapper;

namespace AccountManagement.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, EditUserViewModel>();
        //CreateMap<User, UserViewModel>();
        CreateMap<User, UserViewModel>()
    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));

    }
}
