using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Payment
{
    public interface IPaymentApplication
    {
        Task<long> CreateAsync(CreatePaymentCommand command);
        Task UpdateAsync(EditPaymentCommand command);
        Task CancelAsync(long id);
        Task<PaymentViewModel?> GetByIdAsync(long id);
        Task<List<PaymentViewModel>> GetByCartIdAsync(long cartId);
        Task<List<PaymentViewModel>> SearchAsync(PaymentSearchCriteria searchModel);
    }
}
