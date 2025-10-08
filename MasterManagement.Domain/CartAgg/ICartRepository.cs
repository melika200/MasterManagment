using _01_FrameWork.Domain;
using MasterManagement.Domain.CartAgg;
using MasterManagment.Application.Contracts.OrderContracts;

namespace MasterManagement.Domain.OrderAgg;

public interface ICartRepository : IRepository<long, Cart>
{
    Task<Cart?> GetCartWithItemAsync(long cartId);
    Task<List<Cart>> SearchAsync(CartSearchCriteria criteria);
}

