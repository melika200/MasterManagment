
using MasterManagment.Application.Contracts.Payment;
using MasterManagement.Domain.OrderAgg;
using _01_FrameWork.Application;

namespace MasterManagment.Application
{
    public class PaymentApplication : IPaymentApplication
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfwork;
        private readonly ICartRepository _cartRepository;
        public PaymentApplication(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork, ICartRepository cartRepository = null)
        {
            _paymentRepository = paymentRepository;
            _unitOfwork = unitOfWork;
            _cartRepository = cartRepository;
        }

        public async Task<long> CreateAsync(CreatePaymentCommand command)
        {
            
            var payments = await _paymentRepository.GetManyAsync(p => p.CartId == command.CartId && p.IsSucceeded);
            double totalPaid = payments.Sum(p => p.Amount);

           
            double orderAmount = await GetOrderPayAmountByCartId(command.CartId);

            if (totalPaid + command.Amount > orderAmount)
            {
                throw new Exception("شما قبلاً تمام مبلغ سفارش را پرداخت کرده‌اید. پرداخت بیشتر امکان‌پذیر نیست.");
            }

            var payment = new Payment(
                command.CartId,
                command.Amount,
                command.TransactionId,
                command.IsSucceeded);

            await _paymentRepository.CreateAsync(payment);
            await _unitOfwork.CommitAsync();
            return payment.Id;
        }

        

        public async Task<double> GetOrderPayAmountByCartId(long cartId)
        {
            var cart = await _cartRepository.GetCartDetailsAsync(cartId);
            if (cart == null)
                throw new Exception("سبد خرید یافت نشد.");

            return cart.PayAmount; 
        }

        public async Task UpdateAsync(EditPaymentCommand command)
        {
            var payment = await _paymentRepository.GetAsync(command.Id);
            if (payment == null)
                throw new Exception($"Payment with id {command.Id} not found.");

            payment.Update(command.CartId, command.Amount, command.TransactionId, command.IsSucceeded);
            await _paymentRepository.UpdateAsync(payment);
            await _unitOfwork.CommitAsync();
        }

        public async Task CancelAsync(long id)
        {
            var payment = await _paymentRepository.GetAsync(id);
            if (payment == null)
                throw new Exception($"Payment with id {id} not found.");

            payment.Cancel();
            await _paymentRepository.UpdateAsync(payment);
            await _unitOfwork.CommitAsync();
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

            return payments.Select(payment => new PaymentViewModel
            {
                Id = payment.Id,
                CartId = payment.CartId,
                Amount = payment.Amount,
                TransactionId = payment.TransactionId,
                IsSucceeded = payment.IsSucceeded
            }).ToList();
        }
    }
}
