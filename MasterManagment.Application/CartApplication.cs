using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterManagement.Domain.CartAgg;
using MasterManagment.Application.Contracts.Order;
using MasterManagement.Domain.ProductAgg;
using MasterManagement.Domain.OrderAgg;
using _01_FrameWork.Application;

namespace MasterManagment.Application
{
    public class CartApplication : ICartApplication
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CartApplication(ICartRepository cartRepository, IProductRepository productRepository,IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> CreateAsync(CreateCartCommand command)
        {
            var operation = new OperationResult();

            var existingCart = await _cartRepository.ExistsAsync(c => c.AccountId == command.AccountId && !c.IsCanceled && !c.IsPaid);
            if (existingCart)
                return operation.Failed("برای این حساب کاربری سبد خرید فعالی وجود دارد");

            foreach (var item in command.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                if (product == null)
                    return operation.Failed($"محصول با شناسه {item.ProductId} یافت نشد");
                if (!product.IsAvailable)
                    return operation.Failed($"محصول {product.Name} در دسترس نیست");
                if (item.Count <= 0)
                    return operation.Failed($"تعداد محصول {product.Name} باید بیشتر از صفر باشد");
            }

            var cart = new Cart(
                command.AccountId,
                (PaymentMethod)command.PaymentMethod,
                0, 0, 0);

            foreach (var item in command.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                var cartItem = new CartItem(
                    product.Id,
                    product.Name,
                    (double)product.Price,
                    item.Count,
                    item.DiscountRate);

                cart.AddItem(cartItem);
            }

            await _cartRepository.CreateAsync(cart);
            await _unitOfWork.CommitAsync();

            return operation.Succedded("سبد خرید با موفقیت ایجاد شد");
        }

        public async Task<OperationResult> EditAsync(EditCartCommand command)
        {
            var operation = new OperationResult();

            var cart = await _cartRepository.GetAsync(command.Id);
            if (cart == null)
                return operation.Failed($"سبد خرید با شناسه {command.Id} یافت نشد");

            foreach (var item in command.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                if (product == null)
                    return operation.Failed($"محصول با شناسه {item.ProductId} یافت نشد");
                if (!product.IsAvailable)
                    return operation.Failed($"محصول {product.Name} در دسترس نیست");
                if (item.Count <= 0)
                    return operation.Failed($"تعداد محصول {product.Name} باید بیشتر از صفر باشد");
            }

            cart.Edit((PaymentMethod)command.PaymentMethod, 0, 0, 0);
            cart.ClearItems();

            foreach (var item in command.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                var cartItem = new CartItem(
                    product.Id,
                    product.Name,
                    (double)product.Price,
                    item.Count,
                    item.DiscountRate);

                cart.AddItem(cartItem);
            }

            await _cartRepository.UpdateAsync(cart);
            await _unitOfWork.CommitAsync();

            return operation.Succedded("سبد خرید با موفقیت ویرایش شد");
        }


        //public async Task<long> CreateAsync(CreateCartCommand command)
        //{
        //    var cart = new Cart(
        //        command.AccountId,
        //        (PaymentMethod)command.PaymentMethod,
        //        0, 0, 0);

        //    foreach (var item in command.Items)
        //    {
        //        var product = await _productRepository.GetAsync(item.ProductId);
        //        if (product == null)
        //            throw new Exception($"Product with id {item.ProductId} not found.");

        //        var cartItem = new CartItem(
        //            product.Id,
        //            product.Name,
        //            (double)product.Price,
        //            item.Count,
        //            item.DiscountRate);

        //        cart.AddItem(cartItem);
        //    }

        //    await _cartRepository.CreateAsync(cart);
        //    await _unitOfWork.CommitAsync();
        //    return cart.Id;
        //}

        //public async Task EditAsync(EditCartCommand command)
        //{
        //    var cart = await _cartRepository.GetAsync(command.Id);
        //    if (cart == null)
        //        throw new Exception($"Cart with id {command.Id} not found.");

        //    cart.Edit(
        //        (PaymentMethod)command.PaymentMethod,
        //        0,
        //        0,
        //        0);

        //    cart.ClearItems();

        //    foreach (var item in command.Items)
        //    {
        //        var product = await _productRepository.GetAsync(item.ProductId);
        //        if (product == null)
        //            throw new Exception($"Product with id {item.ProductId} not found.");

        //        var cartItem = new CartItem(
        //            product.Id,
        //            product.Name,
        //            (double)product.Price,
        //            item.Count,
        //            item.DiscountRate);

        //        cart.AddItem(cartItem);
        //    }

        //    await _cartRepository.UpdateAsync(cart);
        //    await _unitOfWork.CommitAsync();
        //}



        public async Task CancelAsync(long id)
        {
            var cart = await _cartRepository.GetAsync(id);
            if (cart == null)
                throw new Exception($"Cart with id {id} not found.");

            cart.Cancel();
            await _cartRepository.UpdateAsync(cart);
            await _unitOfWork.CommitAsync();
        }

        public async Task<double> GetAmountByAsync(long id)
        {
            var cart = await _cartRepository.GetAsync(id);
            if (cart == null)
                throw new Exception($"Cart with id {id} not found.");

            return cart.PayAmount;
        }

        public async Task<List<CartItemViewModel>> GetItemsAsync(long cartId)
        {
            var cart = await _cartRepository.GetAsync(cartId);
            if (cart == null)
                throw new Exception($"Cart with id {cartId} not found.");

            return cart.Items.Select(i => new CartItemViewModel
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Count = i.Count,
                UnitPrice = i.UnitPrice,
                DiscountRate = i.DiscountRate
            }).ToList();
        }

        public async Task<List<CartViewModel>> SearchAsync(CartSearchCriteria searchModel)
        {
            var carts = await _cartRepository.GetAllAsync();
            var query = carts.AsQueryable();

            if (searchModel.AccountId != 0)
                query = query.Where(c => c.AccountId == searchModel.AccountId);

            if (searchModel.IsCanceled.HasValue)
                query = query.Where(c => c.IsCanceled == searchModel.IsCanceled.Value);

            if (searchModel.IsPaid.HasValue)
                query = query.Where(c => c.IsPaid == searchModel.IsPaid.Value);

            return query.Select(c => new CartViewModel
            {
                Id = c.Id,
                AccountName = "", // بارگذاری نام کاربر از سرویس مربوطه پیشنهاد می‌شود
                PaymentMethod = (int)c.PaymentMethod, 
                TotalAmount = c.TotalAmount,
                IsPaid = c.IsPaid,
                IsCanceled = c.IsCanceled,
                IssueTrackingNo = c.IssueTrackingNo
            }).ToList();
        }
    }
}
