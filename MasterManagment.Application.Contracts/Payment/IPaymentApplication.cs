using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Application;

namespace MasterManagment.Application.Contracts.Payment
{
    public interface IPaymentApplication
    {
        Task<OperationResult> CreateAsync(CreatePaymentCommand command);
        Task<OperationResult> UpdateAsync(EditPaymentCommand command);
        Task CancelAsync(long id);
        Task<PaymentViewModel?> GetByIdAsync(long id);
        Task<List<PaymentViewModel>> GetByCartIdAsync(long cartId);
        Task<List<PaymentViewModel>> SearchAsync(PaymentSearchCriteria searchModel);
    }
}
