using _01_FrameWork.Application;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Domain.PaymentAgg;
using MasterManagment.Application.Contracts.Payment;
using MasterManagment.Application.Contracts.UnitOfWork;

public class PaymentApplication : IPaymentApplication
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMasterUnitOfWork _unitOfWork;
    private readonly ICartRepository _cartRepository;

    public PaymentApplication(IPaymentRepository paymentRepository, IMasterUnitOfWork unitOfWork, ICartRepository cartRepository)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _cartRepository = cartRepository;
    }

    //public async Task<OperationResult> CreateAsync(CreatePaymentCommand command)
    //{
    //    var operation = new OperationResult();

    //    var payments = await _paymentRepository.GetManyAsync(p => p.CartId == command.CartId && p.IsSucceeded);
    //    double totalPaid = payments.Sum(p => p.Amount);
    //    double orderAmount;

    //    try
    //    {
    //        orderAmount = await GetOrderPayAmountByCartId(command.CartId);
    //    }
    //    catch (Exception ex)
    //    {
    //        return operation.Failed(ex.Message);
    //    }

    //    if (totalPaid + command.Amount > orderAmount)
    //        return operation.Failed("شما قبلاً تمام مبلغ سفارش را پرداخت کرده‌اید. پرداخت بیشتر امکان‌پذیر نیست.");

    //    var payment = new Payment(
    //        command.CartId,
    //        command.Amount,
    //        command.TransactionId!,
    //        command.IsSucceeded);

    //    await _paymentRepository.CreateAsync(payment);
    //    await _unitOfWork.CommitAsync();

    //    return operation.Succedded("پرداخت با موفقیت ثبت شد");
    //}

    public async Task<OperationResultWithId> CreateAsync(CreatePaymentCommand command)
    {
        var operation = new OperationResultWithId();

        var payments = await _paymentRepository.GetManyAsync(p => p.CartId == command.CartId && p.IsSucceeded);
        double totalPaid = payments.Sum(p => p.Amount);
        double orderAmount;

        try
        {
            orderAmount = await GetOrderPayAmountByCartId(command.CartId);
        }
        catch (Exception ex)
        {
            return operation.Failed(ex.Message);
        }

        if (totalPaid + command.Amount > orderAmount)
            return operation.Failed("شما قبلاً تمام مبلغ سفارش را پرداخت کرده‌اید.");

        var payment = new Payment(
            command.CartId,
            command.Amount,
            command.TransactionId!,
            command.IsSucceeded);

        await _paymentRepository.CreateAsync(payment);
        await _unitOfWork.CommitAsync();

        return operation.Succedded(payment.Id, "پرداخت با موفقیت ثبت شد");
    }

    public async Task<OperationResult> UpdateAsync(EditPaymentCommand command)
    {
        var operation = new OperationResult();

        var payment = await _paymentRepository.GetAsync(command.Id);
        if (payment == null)
            return operation.Failed($"پرداخت با شناسه {command.Id} یافت نشد.");

        payment.Update(command.CartId, command.Amount, command.TransactionId!, command.IsSucceeded);
        await _paymentRepository.UpdateAsync(payment);
        await _unitOfWork.CommitAsync();

        return operation.Succedded("ویرایش پرداخت با موفقیت انجام شد");
    }

    public async Task CancelAsync(long id)
    {
        var payment = await _paymentRepository.GetAsync(id);
        if (payment == null)
            throw new Exception($"پرداخت با شناسه {id} یافت نشد.");

        payment.Cancel();
        await _paymentRepository.UpdateAsync(payment);
        await _unitOfWork.CommitAsync();
    }

    public async Task<PaymentViewModel?> GetByIdAsync(long id)
    {
        var payment = await _paymentRepository.GetAsync(id);
        if (payment == null)
            return null;

        return new PaymentViewModel
        {
            Id = payment.Id,
            CartId = payment.CartId,
            Amount = payment.Amount,
            TransactionId = payment.TransactionId,
            IsSucceeded = payment.IsSucceeded
        };
    }

    public async Task<List<PaymentViewModel>> GetByCartIdAsync(long cartId)
    {
        var payments = await _paymentRepository.GetManyAsync(p => p.CartId == cartId);
        return payments.Select(payment => new PaymentViewModel
        {
            Id = payment.Id,
            CartId = payment.CartId,
            Amount = payment.Amount,
            TransactionId = payment.TransactionId,
            IsSucceeded = payment.IsSucceeded
        }).ToList();
    }

    public async Task<List<PaymentViewModel>> SearchAsync(PaymentSearchCriteria searchModel)
    {
        var payments = await _paymentRepository.GetManyAsync(p =>
            (searchModel.CartId == 0 || p.CartId == searchModel.CartId) &&
            (!searchModel.IsSucceeded.HasValue || p.IsSucceeded == searchModel.IsSucceeded) &&
            (string.IsNullOrEmpty(searchModel.TransactionId) || p.TransactionId == searchModel.TransactionId));

        return payments.Select(p => new PaymentViewModel
        {
            Id = p.Id,
            CartId = p.CartId,
            Amount = p.Amount,
            TransactionId = p.TransactionId,
            IsSucceeded = p.IsSucceeded
        }).ToList();
    }

    public async Task<double> GetOrderPayAmountByCartId(long cartId)
    {
        var cart = await _cartRepository.GetCartWithItemAsync(cartId);
        if (cart == null)
            throw new Exception("سبد خرید یافت نشد.");

        return cart.PayAmount;
    }

    public async Task<OperationResult> ConfirmAsync(ConfirmPaymentCommand command)
    {
        var operation = new OperationResult();
        var payment = await _paymentRepository.GetPaymentByIdAsync(command.PaymentId);
        if (payment == null)
            return operation.Failed("پرداخت یافت نشد");

        payment.MarkAsConfirmed();
        payment.SetAsSucceeded();
        await _unitOfWork.CommitAsync();

        return operation.Succedded("پرداخت تأیید شد");
    }



    public async Task<PaymentViewModel?> GetByTransactionIdAsync(string transactionId)
    {
        var payment = await _paymentRepository.GetByTransactionIdAsync(transactionId);
        if (payment == null) return null;

        return new PaymentViewModel
        {
            Id = payment.Id,
            CartId=payment.CartId,
            Amount = payment.Amount,
            TransactionId = payment.TransactionId,
            IsSucceeded = payment.IsSucceeded
           
        };
    }


}
