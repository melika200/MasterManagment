using _01_FrameWork.Application;

namespace MasterManagment.Application.Contracts.Payment;

public interface IPaymentApplication
{
    Task<OperationResultWithId> CreateAsync(CreatePaymentCommand command);
    Task<OperationResult> UpdateAsync(EditPaymentCommand command);
    Task CancelAsync(long id);
    Task<PaymentViewModel?> GetByIdAsync(long id);
    Task<List<PaymentViewModel>> GetByCartIdAsync(long cartId);
    Task<List<PaymentViewModel>> SearchAsync(PaymentSearchCriteria searchModel);
    Task<OperationResult> ConfirmAsync(ConfirmPaymentCommand command);
    Task<PaymentViewModel?> GetByTransactionIdAsync(string transactionId);



}
