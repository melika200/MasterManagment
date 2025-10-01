using System.Linq.Expressions;
using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.CartAgg;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using MasterManagment.Application.Contracts.Order;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repository
{
    public class CartRepository : RepositoryBase<long, Cart>,ICartRepository
    {
        private readonly MasterContext _context;
        private readonly DbSet<Cart> _dbSet;

        public CartRepository(MasterContext context):base(context)
        {
            _context = context;
            _dbSet = _context.Set<Cart>();
        }

       
       

        public async Task<Cart?> GetCartWithItemAsync(long cartId)
        {
            return await _dbSet.Include(c => c.Items)
                               .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task<List<Cart>> SearchAsync(CartSearchCriteria criteria)
        {
            IQueryable<Cart> query = _dbSet.Include(c => c.Items);

            if (criteria.AccountId != 0)
                query = query.Where(c => c.AccountId == criteria.AccountId);

            if (criteria.IsCanceled.HasValue)
                query = query.Where(c => c.IsCanceled == criteria.IsCanceled.Value);

            if (criteria.IsPaid.HasValue)
                query = query.Where(c => c.IsPaid == criteria.IsPaid.Value);

            return await query.ToListAsync();
        }

        
    }
}
