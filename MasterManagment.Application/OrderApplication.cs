using _01_FrameWork.Application;
using MasterManagement.Domain.CartAgg;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Domain.ProductAgg;
using MasterManagment.Application.Contracts.Order;

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

    public async Task<OperationResult> CreateAsync(CreateOrderCommand command)
    {
        var operation = new OperationResult();

        foreach (var item in command.Items)
        {
            var product = await _productRepository.GetAsync(item.ProductId);
            if (product == null)
                return operation.Failed($"محصول با شناسه {item.ProductId} یافت نشد");
        }

        var order = new Order(command.AccountId,
                              (PaymentMethod)command.PaymentMethod,
                              command.TotalAmount,
                              command.DiscountAmount,
                              command.PayAmount);

        foreach (var item in command.Items)
        {
            var product = await _productRepository.GetAsync(item.ProductId);
            var orderItem = new OrderItem(product.Id,
                                          item.Count,
                                          (double)product.Price,
                                          item.DiscountRate,
                                          product.Name);

            order.AddItem(orderItem);
        }

        await _orderRepository.CreateAsync(order);
        await _unitOfWork.CommitAsync();

        return operation.Succedded();
    }

    public async Task<OperationResult> EditAsync(EditOrderCommand command)
    {
        var operation = new OperationResult();

        var order = await _orderRepository.GetAsync(command.Id);
        if (order == null)
            return operation.Failed($"سفارش با شناسه {command.Id} یافت نشد");

        foreach (var item in command.Items)
        {
            var product = await _productRepository.GetAsync(item.ProductId);
            if (product == null)
                return operation.Failed($"محصول با شناسه {item.ProductId} یافت نشد");
        }

        order.Edit((PaymentMethod)command.PaymentMethod,
                   command.TotalAmount,
                   command.DiscountAmount,
                   command.PayAmount);

        order.ClearItems();

        foreach (var item in command.Items)
        {
            var product = await _productRepository.GetAsync(item.ProductId);
            var orderItem = new OrderItem(product.Id,
                                          item.Count,
                                          (double)product.Price,
                                          item.DiscountRate,
                                          product.Name);

            order.AddItem(orderItem);
        }

        await _orderRepository.UpdateAsync(order);
        await _unitOfWork.CommitAsync();

        return operation.Succedded();
    }

    public async Task CancelAsync(long id)
    {
        var order = await _orderRepository.GetAsync(id);
        if (order == null)
            throw new Exception($"سفارش با شناسه {id} یافت نشد");

        order.Cancel();
        await _orderRepository.UpdateAsync(order);
        await _unitOfWork.CommitAsync();
    }

    public async Task<double> GetAmountByAsync(long id)
    {
        var order = await _orderRepository.GetAsync(id);
        if (order == null)
            throw new Exception($"سفارش با شناسه {id} یافت نشد");

        return order.PayAmount;
    }

    public async Task<List<OrderItemViewModel>> GetItemsAsync(long orderId)
    {
        var order = await _orderRepository.GetAsync(orderId);
        if (order == null)
            throw new Exception($"سفارش با شناسه {orderId} یافت نشد");

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
            AccountName = "",
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

        var payments = await _paymentRepository.GetManyAsync(p => p.CartId == cartId && p.IsSucceeded);
        double totalPaidAmount = payments.Sum(p => p.Amount);

        if (totalPaidAmount < cart.PayAmount)
            throw new Exception("پرداخت کامل برای این سبد ثبت نشده است.");

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

        cart.Cancel();
        await _cartRepository.UpdateAsync(cart);

        await _unitOfWork.CommitAsync();

        return order.Id;
    }
}
