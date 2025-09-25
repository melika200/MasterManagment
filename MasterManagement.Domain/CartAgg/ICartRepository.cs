using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Domain;
using MasterManagement.Domain.CartAgg;
using MasterManagement.Domain.OrderAgg;
using MasterManagment.Application.Contracts.Order;

namespace MasterManagement.Domain.OrderAgg
{
    public interface ICartRepository : IRepository<long, Cart>
    {
        Task<Cart?> GetCartDetailsAsync(long cartId);
        Task<List<Cart>> SearchAsync(CartSearchCriteria criteria);
    }
}

