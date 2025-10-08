using _01_FrameWork.Domain;

namespace MasterManagement.Domain.OrderStateAgg;

public interface IOrderStateRepository : IRepository<long, OrderState>
{
}
