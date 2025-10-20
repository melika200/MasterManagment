using AutoMapper;
using DiscountManagement.Application.Contracts.Discount;
using DiscountManagement.Domain.DiscountAgg;

namespace DiscountManagement.Application;

public class DiscountMappingProfile : Profile
{
    public DiscountMappingProfile()
    {
        CreateMap<Discount, DiscountViewModel>();
        CreateMap<Discount, EditDiscountViewModel>();
    }
}
