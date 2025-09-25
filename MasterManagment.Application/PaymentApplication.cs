
using MasterManagment.Application.Contracts.Payment;
using MasterManagement.Domain.OrderAgg;
using _01_FrameWork.Application;

namespace MasterManagment.Application
{
    public class PaymentApplication : IPaymentApplication
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfwork;

        public PaymentApplication(IPaymentRepository paymentRepository,IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _unitOfwork = unitOfWork;
        }

        public async Task<long> CreateAsync(CreatePaymentCommand command)
        {
            var payment = new Payment(
                command.OrderId,
                command.Amount,
                command.TransactionId,
                command.IsSucceeded);

            await _paymentRepository.CreateAsync(payment);
            await _unitOfwork.CommitAsync();
            return payment.Id;
        }

        public async Task UpdateAsync(EditPaymentCommand command)
        {
            var payment = await _paymentRepository.GetAsync(command.Id);
            if (payment == null)
                throw new Exception($"Payment with id {command.Id} not found.");

            payment.Update(command.OrderId, command.Amount, command.TransactionId, command.IsSucceeded);
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
                OrderId = payment.OrderId,
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
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                TransactionId = payment.TransactionId,
                IsSucceeded = payment.IsSucceeded
            }).ToList();
        }


        public async Task<List<PaymentViewModel>> SearchAsync(PaymentSearchCriteria searchModel)
        {
            var payments = await _paymentRepository.GetManyAsync(p =>
                (searchModel.OrderId == 0 || p.OrderId == searchModel.OrderId) &&
                (!searchModel.IsSucceeded.HasValue || p.IsSucceeded == searchModel.IsSucceeded) &&
                (string.IsNullOrEmpty(searchModel.TransactionId) || p.TransactionId == searchModel.TransactionId));

            return payments.Select(payment => new PaymentViewModel
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                TransactionId = payment.TransactionId,
                IsSucceeded = payment.IsSucceeded
            }).ToList();
        }
    }
}
