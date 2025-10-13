using _01_FrameWork.Application;
using MasterManagement.Domain.CartAgg;
using MasterManagement.Domain.OrderAgg;
using MasterManagement.Domain.PaymentMethodsTypeAgg;
using MasterManagement.Domain.ProductAgg;
using MasterManagment.Application.Contracts.CartItem;
using MasterManagment.Application.Contracts.OrderContracts;
using MasterManagment.Application.Contracts.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace MasterManagment.Application
{
    public class CartApplication : ICartApplication
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMasterUnitOfWork _unitOfWork;

        public CartApplication(ICartRepository cartRepository, IProductRepository productRepository,IMasterUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> CreateAsync(CreateCartCommand command)
        {
            var operation = new OperationResult();

            try
            {
               
                var existingCart = await _cartRepository.IsExistsAsync(c =>
                    c.AccountId == command.AccountId && !c.IsCanceled && !c.IsPaid);

                if (existingCart)
                    return operation.Failed("برای این حساب کاربری سبد خرید فعالی وجود دارد");


              
                var productIds = command.Items!.Select(i => i.ProductId).ToList();
                var products = await _productRepository.GetListProductByIdsAsync(productIds);

               
                foreach (var item in command.Items)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product == null)
                        return operation.Failed($"محصول با شناسه {item.ProductId} یافت نشد");
                    if (!product.IsAvailable)
                        return operation.Failed($"محصول {product.Name} در دسترس نیست");
                    if (item.Count <= 0)
                        return operation.Failed($"تعداد محصول {product.Name} باید بیشتر از صفر باشد");
                }

            
                var method = PaymentMethodsType.AllMethods.FirstOrDefault(m => m.Id == command.PaymentMethodId);
                if (method == null)
                    return operation.Failed("روش پرداخت معتبر نیست");

                var cart = new Cart(
                    command.AccountId,
                    method.Id,
                    method.Name,
                    0, 0, 0);

          
                foreach (var item in command.Items)
                {
                    var product = products.First(p => p.Id == item.ProductId);

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
            catch (DbUpdateException dbEx)
            {
                
                return operation.Failed($"خطای پایگاه داده: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
              
                return operation.Failed($"خطای غیرمنتظره: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<OperationResult> EditAsync(EditCartCommand command)
        {
            var operation = new OperationResult();

            var cart = await _cartRepository.GetAsync(command.Id);
            if (cart == null)
                return operation.Failed($"سبد خرید با شناسه {command.Id} یافت نشد");

            foreach (var item in command.Items!)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                if (product == null)
                    return operation.Failed($"محصول با شناسه {item.ProductId} یافت نشد");
                if (!product.IsAvailable)
                    return operation.Failed($"محصول {product.Name} در دسترس نیست");
                if (item.Count <= 0)
                    return operation.Failed($"تعداد محصول {product.Name} باید بیشتر از صفر باشد");
            }

            //cart.Edit((PaymentMethod)command.PaymentMethod, 0, 0, 0);
            var method = PaymentMethodsType.AllMethods.FirstOrDefault(m => m.Id == command.PaymentMethodId);
            if (method == null)
                return operation.Failed("روش پرداخت معتبر نیست");

            cart.Edit(method.Id,method.Name, 0, 0, 0);

            cart.ClearItems();

            foreach (var item in command.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                var cartItem = new CartItem(
                    product!.Id,
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
            var cart = await _cartRepository.GetCartWithItemAsync(cartId);
            if (cart == null)
                throw new Exception($"Cart with id {cartId} not found.");

            return cart.Items.Select(i => new CartItemViewModel
            {
                ProductId = i.ProductId,
                ProductName=i.ProductName,
                //ProductName = i.ProductName,
                Count = i.Count,
                //UnitPrice = i.UnitPrice,
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
                AccountId=c.AccountId,
                //AccountName = c.AccountName,
                PaymentMethodId = c.PaymentMethodId,
                PaymentMethodName = c.PaymentMethodName,
                TotalAmount = c.TotalAmount,
                IsPaid = c.IsPaid,
                IsCanceled = c.IsCanceled,
                IssueTrackingNo = c.IssueTrackingNo.ToString()


            }).ToList();
        }

        public async Task<OperationResult> DeleteAsync(long cartid)
        {
            var operation = new OperationResult();

            var product = await _cartRepository.GetAsync(cartid);
            if (product == null)
                return operation.Failed("سبدی  یافت نشد");
            product.SoftDelete();

            await _unitOfWork.CommitAsync();

            return operation.Succedded("سبد خرید با موفقیت حذف شد");
        }


        public async Task<CartViewModel?> GetCartByIdAsync(long cartId)
        {
            var cart = await _cartRepository.GetAsync(cartId);
            if (cart == null) return null;

            return new CartViewModel
            {
                Id = cart.Id,
                AccountId = cart.AccountId,
                PaymentMethodId = cart.PaymentMethodId,
                PaymentMethodName = cart.PaymentMethodName,
                DiscountAmount = cart.DiscountAmount,
                TotalAmount = cart.PayAmount
           
            };
        }


    }
}
