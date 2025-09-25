using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterManagement.Domain.CartAgg;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Domain.ProductAgg;
using MasterManagment.Application.Contracts.Order;
using _01_FrameWork.Application;

namespace MasterManagment.Application
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderApplication(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository,
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> CreateAsync(CreateOrderCommand command)
        {
            var order = new Order(command.AccountId,
                                  (PaymentMethod)command.PaymentMethod,
                                  command.TotalAmount,
                                  command.DiscountAmount,
                                  command.PayAmount);

            foreach (var item in command.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product with id {item.ProductId} not found.");

                var orderItem = new OrderItem(product.Id,
                                              item.Count,
                                              (double)product.Price,
                                              item.DiscountRate,
                                              product.Name);

                order.AddItem(orderItem);
            }

            await _orderRepository.CreateAsync(order);
            await _unitOfWork.CommitAsync();

            return order.Id;
        }

        public async Task EditAsync(EditOrderCommand command)
        {
            var order = await _orderRepository.GetAsync(command.Id);
            if (order == null)
                throw new Exception($"Order with id {command.Id} not found.");

            order.Edit((PaymentMethod)command.PaymentMethod, command.TotalAmount, command.DiscountAmount, command.PayAmount);

            order.ClearItems();

            foreach (var item in command.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product with id {item.ProductId} not found.");

                var orderItem = new OrderItem(product.Id,
                                              item.Count,
                                              (double)product.Price,
                                              item.DiscountRate,
                                              product.Name);

                order.AddItem(orderItem);
            }

            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.CommitAsync();
        }

        public async Task CancelAsync(long id)
        {
            var order = await _orderRepository.GetAsync(id);
            if (order == null)
                throw new Exception($"Order with id {id} not found.");

            order.Cancel();
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.CommitAsync();
        }

        public async Task<double> GetAmountByAsync(long id)
        {
            var order = await _orderRepository.GetAsync(id);
            if (order == null)
                throw new Exception($"Order with id {id} not found.");

            return order.PayAmount;
        }

        public async Task<List<OrderItemViewModel>> GetItemsAsync(long orderId)
        {
            var order = await _orderRepository.GetAsync(orderId);
            if (order == null)
                throw new Exception($"Order with id {orderId} not found.");

            return order.Items.Select(i => new OrderItemViewModel
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Count = i.Count,
                UnitPrice = i.UnitPrice,
                DiscountRate = i.DiscountRate
            }).ToList();
        }

        public async Task<List<OrderViewModel>> SearchAsync(OrderSearchCriteria searchModel)
        {
            var orders = await _orderRepository.GetAllAsync();
            var query = orders.AsQueryable();

            if (searchModel.AccountId != 0)
                query = query.Where(o => o.AccountId == searchModel.AccountId);

            if (searchModel.IsCanceled.HasValue)
                query = query.Where(o => o.IsCanceled == searchModel.IsCanceled.Value);

            if (searchModel.IsPaid.HasValue)
                query = query.Where(o => o.IsPaid == searchModel.IsPaid.Value);

            return query.Select(o => new OrderViewModel
            {
                Id = o.Id,
                AccountName = "", // می‌توانید از سرویس دیگر بارگذاری شود
                PaymentMethod = (int)o.PaymentMethod,
                TotalAmount = o.TotalAmount,
                IsPaid = o.IsPaid,
                IsCanceled = o.IsCanceled,
                IssueTrackingNo = o.IssueTrackingNo
            }).ToList();
        }

        public async Task<long> FinalizeFromCartAsync(long cartId, string transactionId)
        {
            var cart = await _cartRepository.GetCartDetailsAsync(cartId);
            if (cart == null)
                throw new Exception("سبد خرید یافت نشد.");

            if (!cart.IsPaid)
                throw new Exception("پرداخت برای این سبد ثبت نشده است.");

            var payment = await _paymentRepository.GetByTransactionIdAsync(transactionId);
            if (payment == null || !payment.IsSucceeded)
                throw new Exception("پرداخت موفق ثبت نشده است.");

            var order = new Order(cart.AccountId,
                                  cart.PaymentMethod,
                                  cart.TotalAmount,
                                  cart.DiscountAmount,
                                  cart.PayAmount);

            foreach (var item in cart.Items)
            {
                var orderItem = new OrderItem(item.ProductId,
                                              item.Count,
                                              item.UnitPrice,
                                              item.DiscountRate,
                                              item.ProductName);
                order.AddItem(orderItem);
            }

            await _orderRepository.CreateAsync(order);

            // لغو سبد خرید برای جلوگیری از استفاده مجدد
            cart.Cancel();
            await _cartRepository.UpdateAsync(cart);

            await _unitOfWork.CommitAsync();

            return order.Id;
        }
    }
}
