using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Application;
using MasterManagement.Domain.ProductCategoryAgg;
using MasterManagment.Application.Contracts.ProductCategory;

namespace MasterManagment.Application
{
    public class ProductCategoryApplication : IProductCategoryApplication
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryApplication(IProductCategoryRepository ProductCategoryRepository, IUnitOfWork unitOfWork)
        {
            _productCategoryRepository = ProductCategoryRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> CreateAsync(CreateProductCategory command)
        {
            var operation = new OperationResult();
            if (_productCategoryRepository.Exists(x=>x.Name == command.Name))
                return operation.Failed("امکان ثبت رکورد تکراری وجود ندارد .لطفا مجدد تلاش کنید");

            var slug = command.Slug.Slugify();


            var productCategory = new ProductCategory(command.Name, command.Description,
                 command.Picture, command.PictureAlt, command.PictureTitle, command.Keywords,
                command.MetaDescription, slug);

            _productCategoryRepository.Create(productCategory);

            //_productCategoryRepository.SaveChanges();
            await _unitOfWork.CommitAsync();

            return operation.Succedded();
        }

        public OperationResult Edit(EditProductCategory command)
        {
            var operation = new OperationResult();
            var productCategory = _productCategoryRepository.Get(command.Id);

            if (productCategory == null)
                return operation.Failed("رکورد با اطلاعات درخواست شده یافت نشد .لطفا مجدد تلاش بفرمایید");

            if (_productCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed("امکان ثبت رکورد تکراری وجود ندارد .لطفا مجدد تلاش کنید");

            var slug = command.Slug.Slugify();

          

            productCategory.Edit(command.Name, command.Description,command.Picture,
                command.PictureAlt, command.PictureTitle, command.Keywords,
                command.MetaDescription, slug);

            _productCategoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditProductCategory GetDetails(long id)
        {
            return _productCategoryRepository.GetDetails(id);
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            return _productCategoryRepository.Search(searchModel);
        }

    }
}
