using _01_FrameWork.Application;
using MasterManagement.Domain.ProductAgg;
using MasterManagement.Domain.ProductCategoryAgg;
using MasterManagment.Application.Contracts.Product;

namespace MasterManagment.Application
{
    public class ProductApplication : IProductApplication
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductApplication(
            IProductRepository productRepository,
            IProductCategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
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

        public async Task<OperationResult> Edit(EditProductCommand command)
        {
            var operation = new OperationResult();

            var product = await _productRepository.GetAsync(command.Id);
            if (product == null)
                return operation.Failed("محصول یافت نشد");

            if (await _productRepository.IsExistsAsync(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed("نام محصول تکراری است");

            var category = _categoryRepository.GetById(command.CategoryId).Result;
            if (category == null)
                return operation.Failed("دسته‌بندی انتخاب شده معتبر نیست");

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

            return operation.Succedded("ویرایش محصول با موفقیت انجام شد");
        }

        public async Task<ProductViewModel> GetDetails(long id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
                return null!;

            return new ProductViewModel
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
                CategoryName = product.Category?.Name
            };
        }

        public async Task<List<ProductViewModel>> Search(ProductSearchCriteria searchModel)
        {
            var query = await _productRepository.GetAllWithCategory();

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            if (searchModel.CategoryId.HasValue)
                query = query.Where(x => x.CategoryId == searchModel.CategoryId.Value);

            if (searchModel.IsAvailable.HasValue)
                query = query.Where(x => x.IsAvailable == searchModel.IsAvailable.Value);

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

            var product = await _productRepository.GetById(id);
            if (product == null)
                return operation.Failed("محصول یافت نشد");
            product.SoftDelete();
            //_productRepository.DeleteAsync(product);
            await _unitOfWork.CommitAsync();

            return operation.Succedded("محصول با موفقیت حذف شد");
        }

      
    }
}
