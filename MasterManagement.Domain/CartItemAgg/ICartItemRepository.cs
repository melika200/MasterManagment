using _01_FrameWork.Domain;
using MasterManagement.Domain.OrderAgg;

namespace MasterManagement.Domain.CartItemAgg;

interface ICartItemRepository : IRepository<long, Order>
{
}
