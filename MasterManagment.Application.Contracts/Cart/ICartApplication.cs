using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterManagment.Application.Contracts.Order;

namespace MasterManagment.Application.Contracts.Order
{
    public interface ICartApplication
    {
        Task<long> CreateAsync(CreateCartCommand command);
        Task EditAsync(EditCartCommand command);
        Task CancelAsync(long id);
        Task<double> GetAmountByAsync(long id);
        Task<List<CartItemViewModel>> GetItemsAsync(long cartId);
        Task<List<CartViewModel>> SearchAsync(CartSearchCriteria searchModel);
    }
}


  
 

    


