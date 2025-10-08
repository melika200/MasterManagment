using _01_FrameWork.Application;
using MasterManagement.Domain.CartAgg;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Domain.OrderItemAgg;
using MasterManagement.Domain.OrderStatesTypeAgg;
using MasterManagement.Domain.PaymentAgg;
using MasterManagement.Domain.ProductAgg;
using MasterManagement.Domain.ShippingStatusesTypeAgg;
using MasterManagment.Application.Contracts.OrderContracts;
using MasterManagment.Application.Contracts.OrderItem;
using MasterManagment.Application.Contracts.UnitOfWork;

public class OrderApplication : IOrderApplication
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMasterUnitOfWork _unitOfWork;

    public OrderApplication(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICartRepository cartRepository,
        IPaymentRepository paymentRepository,
        IMasterUnitOfWork unitOfWork)
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

        foreach (var item in command.Items!)
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
            var orderItem = new OrderItem(product!.Id,
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

    public async Task<OperationResult> SetTrackingNumberAsync(long orderId, string trackingNumber)
    {
        var operation = new OperationResult();
        var order = await _orderRepository.GetAsync(orderId);

        if (order == null)
            return operation.Failed("سفارش یافت نشد");

        order.SetIssueTrackingNo(trackingNumber);
        await _orderRepository.UpdateAsync(order);
        await _unitOfWork.CommitAsync();

        return operation.Succedded();
    }



    public async Task<OperationResult> SetOrderStateAsync(long orderId, int newStateId)
    {
        var operation = new OperationResult();
        var order = await _orderRepository.GetAsync(orderId);
        if (order == null)
            return operation.Failed("سفارش یافت نشد");

        var newState = OrderStatesType.AllStates.FirstOrDefault(s => s.Id == newStateId);
        if (newState == null)
            return operation.Failed("وضعیت سفارش معتبر نیست");

        order.SetOrderState(newState);


        await _orderRepository.UpdateAsync(order);
        await _unitOfWork.CommitAsync();

        return operation.Succedded();
    }

    public async Task<OperationResult> SetOrderShippingStateAsync(long orderId, int newShippingStateId)
    {
        var operation = new OperationResult();
        var order = await _orderRepository.GetAsync(orderId);
        if (order == null)
            return operation.Failed("سفارش یافت نشد");

        var newState = ShippingStatusesType.AllStatuses.FirstOrDefault(s => s.Id == newShippingStateId);
        if (newState == null)
            return operation.Failed("وضعیت ارسال معتبر نیست");

        order.SetShippingStatus(newState);


        await _orderRepository.UpdateAsync(order);
        await _unitOfWork.CommitAsync();

        return operation.Succedded();
    }


    public async Task<List<OrderViewModel>> GetOrders()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();

        return orders.Select(u => new OrderViewModel
        {
            Id = u.Id,
            AccountId = u.AccountId,
            FullName = u.FullName,
            Mobile = u.Mobile,
            Address = u.Address,
            PaymentMethod = (int)u.PaymentMethod,
            TotalAmount = u.TotalAmount,
            IsPaid = u.IsPaid,
            IsCanceled = u.IsCanceled,
            IssueTrackingNo = u.IssueTrackingNo,
            OrderState = u.OrderState?.Name ?? "نامشخص",
            ShippingState = u.ShippingStatus?.Name ?? "نامشخص"
        }).ToList();
    }

    public async Task<OperationResult> EditAsync(EditOrderCommand command)
    {
        var operation = new OperationResult();

        var order = await _orderRepository.GetAsync(command.Id);
        if (order == null)
            return operation.Failed($"سفارش با شناسه {command.Id} یافت نشد");

        foreach (var item in command.Items!)
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
            var orderItem = new OrderItem(product!.Id,
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

    //public async Task<List<OrderItemViewModel>> GetOrderItemsAsync(long orderId)
    //{
    //    var order = await _orderRepository.GetOrderWithItemAsync(orderId);
    //    if (order == null)
    //        throw new Exception($"سفارش با شناسه {orderId} یافت نشد");

    //    return order.Items.Select(i => new OrderItemViewModel
    //    {
    //        ProductId = i.ProductId,
    //        ProductName = i.ProductName,
    //        Count = i.Count,
    //        UnitPrice = i.UnitPrice,
    //        DiscountRate = i.DiscountRate
    //    }).ToList();
    //}
    public async Task<OrderDetailViewModel?> GetOrderDetailAsync(long orderId)
    {
        var order = await _orderRepository.GetOrderWithItemAsync(orderId);
        if (order == null)
            return null;

        return new OrderDetailViewModel
        {
            Id = order.Id,
            AccountId = order.AccountId,
            FullName = order.FullName ?? "",
            PhoneNumber = order.Mobile ?? "",
            Address = order.Address ?? "",
            PaymentMethod = (int)order.PaymentMethod,
            TotalAmount = order.TotalAmount,
            IsPaid = order.IsPaid,
            IsCanceled = order.IsCanceled,
            IssueTrackingNo = order.IssueTrackingNo ?? "",
            OrderState = order.OrderState?.Name ?? "نامشخص",
            ShippingState = order.ShippingStatus?.Name ?? "نامشخص",
            Items = order.Items.Select(i => new OrderItemViewModel
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Count = i.Count,
                UnitPrice = i.UnitPrice,
                DiscountRate = i.DiscountRate
            }).ToList()
        };
    }




    public async Task<long> FinalizeFromCartAsync(long cartId, string transactionId)
    {
        var cart = await _cartRepository.GetCartWithItemAsync(cartId);
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
    
    
    
    
    
    public async Task<List<OrderViewModel>> SearchAsync(OrderSearchCriteria searchModel)
    {
        return await _orderRepository.SearchAsync(searchModel);
    }
}
