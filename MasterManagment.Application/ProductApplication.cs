using System.Security.Claims;
using _01_FrameWork.Application;
using MasterManagement.Domain.GalleryAgg;
using MasterManagement.Domain.ProductAgg;
using MasterManagement.Domain.ProductCategoryAgg;
using MasterManagement.Domain.ProductReviewAgg;
using MasterManagment.Application.Contracts.Gallery;
using MasterManagment.Application.Contracts.Product;
using MasterManagment.Application.Contracts.UnitOfWork;

namespace MasterManagment.Application
{
    public class ProductApplication : IProductApplication
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _categoryRepository;
        private readonly IMasterUnitOfWork _unitOfWork;
        private readonly IGalleryRepository _galleryRepository;
        private readonly IProductReviewRepository _productReviewRepository;

        public ProductApplication(
            IProductRepository productRepository,
            IProductCategoryRepository categoryRepository,
            IMasterUnitOfWork unitOfWork,
            IGalleryRepository galleryRepository,
            IProductReviewRepository productReviewRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _galleryRepository = galleryRepository;
            _productReviewRepository = productReviewRepository;
        }

        public async Task<OperationResult> CreateAsync(CreateProductCommand command)
        {
            var operation = new OperationResult();

            if (await _productRepository.IsExistsAsync(x => x.Name == command.Name))
                return operation.Failed("امکان ثبت محصول تکراری وجود ندارد");

            var category = await _categoryRepository.GetById(command.CategoryId);
            if (category == null)
                return operation.Failed("دسته‌بندی انتخاب شده معتبر نیست");

            var product = new Product(
                command.Name!,
                command.ImagePath!,
                command.Price,
                command.Description!,
                command.Stock,
                command.CategoryId,
                command.IsAvailable);

            await _productRepository.CreateAsync(product);
            await _unitOfWork.CommitAsync();

            return operation.Succedded("محصول با موفقیت ثبت شد");
        }

        public async Task<List<ProductViewModel>> GetAllProductsWithCategory()
        {
            var products = await _productRepository.GetAllProductsWithCategory();
            return products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                ImagePath = p.ImagePath,
                Price = p.Price,
                Description = p.Description,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                IsAvailable = p.IsAvailable,
                CategoryName = p.Category?.Name
            }).ToList();
        }

        public async Task<ProductEditResponseCommand> Edit(EditProductCommand command)
        {
            var response = new ProductEditResponseCommand();

            var product = await _productRepository.GetByIdAsync(command.Id);
            if (product == null)
            {
                response.IsSuccedded = false;
                response.Message = "محصول یافت نشد";
                return response;
            }

            if (await _productRepository.IsExistsAsync(x => x.Name == command.Name && x.Id != command.Id))
            {
                response.IsSuccedded = false;
                response.Message = "نام محصول تکراری است";
                return response;
            }

            var category = await _categoryRepository.GetById(command.CategoryId);
            if (category == null)
            {
                response.IsSuccedded = false;
                response.Message = "دسته‌بندی انتخاب شده معتبر نیست";
                return response;
            }

            product.Edit(
                command.Name!,
                command.ImagePath!,
                command.Price,
                command.Description!,
                command.Stock,
                command.CategoryId,
                command.IsAvailable);

            await _productRepository.UpdateAsync(product);
            await _unitOfWork.CommitAsync();

            var galleries = await _galleryRepository.GetManyAsync(g => g.ProductId == product.Id);

            var productViewModel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                ImagePath = product.ImagePath,
                Price = product.Price,
                Description = product.Description,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                IsAvailable = product.IsAvailable,
                TotalRatings = product.TotalRatings,
                AverageRating = product.AverageRating,
                CategoryName = product.Category?.Name,
                Galleries = galleries.Select(g => new GalleryViewModel
                {
                    Id = g.Id,
                    FileName = g.FileName,
                    FilePath = g.FilePath
                }).ToList()
            };

            response.IsSuccedded = true;
            response.Message = "ویرایش محصول با موفقیت انجام شد";
            response.Product = productViewModel;
            return response;
        }


        //public async Task<OperationResult> Edit(EditProductCommand command)
        //{
        //    var operation = new OperationResult();

        //    var product = await _productRepository.GetAsync(command.Id);
        //    if (product == null)
        //        return operation.Failed("محصول یافت نشد");

        //    if (await _productRepository.IsExistsAsync(x => x.Name == command.Name && x.Id != command.Id))
        //        return operation.Failed("نام محصول تکراری است");

        //    var category = _categoryRepository.GetById(command.CategoryId).Result;
        //    if (category == null)
        //        return operation.Failed("دسته‌بندی انتخاب شده معتبر نیست");

        //    product.Edit(
        //        command.Name!,
        //        command.ImagePath!,
        //        command.Price,
        //        command.Description!,
        //        command.Stock,
        //        command.CategoryId,
        //        command.IsAvailable);

        //    await _productRepository.UpdateAsync(product);
        //    await _unitOfWork.CommitAsync();

        //    return operation.Succedded("ویرایش محصول با موفقیت انجام شد");
        //}
        public async Task<ProductViewModel> GetDetails(long id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return null!;

            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                ImagePath = product.ImagePath,
                Price = product.Price,
                Description = product.Description,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                IsAvailable = product.IsAvailable,
                TotalRatings = product.TotalRatings,
                AverageRating = product.AverageRating,
                CategoryName = product.Category?.Name,
                Galleries = product.Galleries?.Select(g => new GalleryViewModel
                {
                    Id = g.Id,
                    FileName = g.FileName,
                    FilePath = g.FilePath
                }).ToList()
            };
        }

        //public async Task<ProductViewModel> GetDetails(long id)
        //{
        //    var product = await _productRepository.GetByIdAsync(id);
        //    if (product == null)
        //        return null!;

        //    return new ProductViewModel
        //    {
        //        Id=product.Id,
        //        Name = product.Name,
        //        ImagePath = product.ImagePath,
        //        Price = product.Price,
        //        Description = product.Description,
        //        Stock = product.Stock,
        //        CategoryId = product.CategoryId,
        //        IsAvailable = product.IsAvailable,
        //        TotalRatings = product.TotalRatings,
        //        AverageRating = product.AverageRating,
        //        CategoryName = product.Category?.Name
        //    };
        //}

        public async Task<List<ProductViewModel>> Search(ProductSearchCriteria searchModel)
        {
            var query = await _productRepository.GetAllProductsWithCategory();

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            //if (searchModel.CategoryId.HasValue)
            //    query = query.Where(x => x.CategoryId == searchModel.CategoryId.Value);

            //if (searchModel.IsAvailable.HasValue)
            //    query = query.Where(x => x.IsAvailable == searchModel.IsAvailable.Value);

            var result = query.Select(product => new ProductViewModel
            {
                Id=product.Id,
                Name = product.Name,
                ImagePath = product.ImagePath,
                Price = product.Price,
                Description = product.Description,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                IsAvailable = product.IsAvailable,
                TotalRatings = product.TotalRatings,
                AverageRating = product.AverageRating,
                CategoryName = product.Category?.Name,
            }).ToList();

            return result;
        }

        public async Task<OperationResult> DeleteAsync(long id)
        {
            var operation = new OperationResult();

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return operation.Failed("محصول یافت نشد");
            product.SoftDelete();
            //_productRepository.DeleteAsync(product);
            await _unitOfWork.CommitAsync();

            return operation.Succedded("محصول با موفقیت حذف شد");
        }

        public async Task<List<ProductViewModel>> GetPopularProducts(int count = 10)
        {
            var products = (await _productRepository.GetAllProductsWithCategory())
                .OrderByDescending(p => p.TotalRatings)
                .ThenByDescending(p => p.AverageRating)
                .Take(count)
                .ToList();

            return products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                ImagePath = p.ImagePath,
                Price = p.Price,
                CategoryId = p.CategoryId,
                IsAvailable = p.IsAvailable,
                TotalRatings = p.TotalRatings,
                AverageRating = p.AverageRating,
                CategoryName = p.Category?.Name
            }).ToList();
        }

        public async Task<List<ProductViewModel>> GetNewestProducts(int count = 10)
        {
            var products = (await _productRepository.GetAllProductsWithCategory())
                .OrderByDescending(p => p.CreatedDate)
                .Take(count)
                .ToList();

            return products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                ImagePath = p.ImagePath,
                Price = p.Price,
                CategoryId = p.CategoryId,
                IsAvailable = p.IsAvailable,
                TotalRatings = p.TotalRatings,
                AverageRating = p.AverageRating,
                CategoryName = p.Category?.Name
            }).ToList();
        }

        public async Task<OperationResult> RateProduct(long productId, int rating, string? comment, ClaimsPrincipal user)
        {
            var operation = new OperationResult();

           
            var userId = AccountUtils.GetAccountId(user);
            if (userId == 0)
                return operation.Failed("کاربر احراز هویت نشده است.");

            string userFullName = user.FindFirst(ClaimTypes.Name)?.Value ?? "ناشناخته";
            string userEmail = user.FindFirst(ClaimTypes.Email)?.Value ?? "";

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return operation.Failed("محصول یافت نشد");

            var review = new ProductReview(productId, userId, userFullName, userEmail, comment ?? "", rating);
            await _productReviewRepository.CreateAsync(review);

            var reviews = await _productReviewRepository.GetByProductIdAsync(productId);
            var totalRatings = reviews.Count;
            var averageRating = reviews.Average(r => r.Rating);

            product.UpdateRatings(totalRatings, averageRating);
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.CommitAsync();

            return operation.Succedded("امتیاز با موفقیت ثبت شد");
        }





    }
}
