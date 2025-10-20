using _01_FrameWork.Infrastructure;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DiscountManagement.Infrastructure.EFCore.Context;
using DiscountManagement.Application.Contracts.Discount;
using DiscountManagement.Domain.DiscountAgg;
using Microsoft.EntityFrameworkCore;

namespace DiscountManagement.Infrastructure.EFCore.Repository;

public class DiscountRepository : RepositoryBase<long, Discount>, IDiscountRepository
{
    private readonly DiscountContext _context;
    private readonly IMapper _mapper;

    public DiscountRepository(DiscountContext context, IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EditDiscountViewModel?> GetForEditAsync(long id)
        => await _context.Discounts
            .Where(x => x.Id == id)
            .ProjectTo<EditDiscountViewModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<DiscountViewModel>> SearchAsync(SearchDiscountCriteria criteria)
    {
        var query = _context.Discounts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(criteria.Title))
            query = query.Where(x => x.Title.Contains(criteria.Title));

        if (criteria.IsActive.HasValue)
            query = query.Where(x => x.IsActive == criteria.IsActive.Value);

        if (criteria.FromDate.HasValue)
            query = query.Where(x => x.StartDate >= criteria.FromDate);

        if (criteria.ToDate.HasValue)
            query = query.Where(x => x.EndDate <= criteria.ToDate);

        return await query
            .OrderByDescending(x => x.Id)
            .ProjectTo<DiscountViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
