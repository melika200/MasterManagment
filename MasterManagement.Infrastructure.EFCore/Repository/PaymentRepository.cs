using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.PaymentAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repository;

public class PaymentRepository : RepositoryBase<long, Payment>, IPaymentRepository
{
    private readonly MasterContext _context;
    private readonly DbSet<Payment> _dbSet;

    public PaymentRepository(MasterContext context):base(context)
    {
        _context = context;
        _dbSet = _context.Set<Payment>();
    }

    public async Task<Payment?> GetByTransactionIdAsync(string transactionId)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.TransactionId == transactionId);
    }

    public async Task<List<Payment>> GetPaymentsByOrderIdAsync(long orderId)
    {
        return await _dbSet.Where(p => p.OrderId == orderId).ToListAsync();
    }



}
