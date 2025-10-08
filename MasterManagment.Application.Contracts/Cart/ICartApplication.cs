using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _01_FrameWork.Application;
using MasterManagment.Application.Contracts.CartItem;

namespace MasterManagment.Application.Contracts.OrderContracts
{
    public interface ICartApplication
    {
        Task<OperationResult> CreateAsync(CreateCartCommand command);
        Task<OperationResult> EditAsync(EditCartCommand command);
        Task CancelAsync(long id);
        Task<double> GetAmountByAsync(long id);
        Task<List<CartItemViewModel>> GetItemsAsync(long cartId);
        Task<List<CartViewModel>> SearchAsync(CartSearchCriteria searchModel);
        Task<OperationResult> DeleteAsync(long id);
    }
}


  
 

    


