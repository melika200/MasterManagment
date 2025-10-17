using _01_FrameWork.Domain;

namespace MasterManagement.Domain.SupportAgg;

public interface ISupportRepository : IRepository<long, Support>
{
    Task<List<Support>> SearchAsync(string? keyword, string? status, long? accountId);
    Task<List<Support>> GetByAccountIdAsync(long accountId);
}
