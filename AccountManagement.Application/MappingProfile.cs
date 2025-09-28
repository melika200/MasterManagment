using AccountManagment.Contracts;
using AccountManagment.Domain.UserAgg;
using AutoMapper;

namespace AccountManagement.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, EditUserViewModel>();
        CreateMap<User, UserViewModel>();
    }
}
