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
        private readonly IProductCategoryRepository _ProductCategoryRepository;
        public ProductCategoryApplication(IProductCategoryRepository ProductCategoryRepository)
        {
            _ProductCategoryRepository = ProductCategoryRepository;
        }
        public OperationResult Create(CreateProductCategory command)
        {
            var operation = new OperationResult();
            if (_ProductCategoryRepository.Exists(x=>x.Name == command.Name))
                return operation.Failed("امکان ثبت رکورد تکراری وجود ندارد .لطفا مجدد تلاش کنید");

            var slug = command.Slug.Slugify();

           
          

            var productCategory = new ProductCategory(command.Name, command.Description,
                 command.Picture, command.PictureAlt, command.PictureTitle, command.Keywords,
                command.MetaDescription, slug);

            _ProductCategoryRepository.Create(productCategory);
            _ProductCategoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditProductCategory command)
        {
            var operation = new OperationResult();
            var productCategory = _ProductCategoryRepository.Get(command.Id);

            if (productCategory == null)
                return operation.Failed("رکورد با اطلاعات درخواست شده یافت نشد .لطفا مجدد تلاش بفرمایید");

            if (_ProductCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed("امکان ثبت رکورد تکراری وجود ندارد .لطفا مجدد تلاش کنید");

            var slug = command.Slug.Slugify();

          

            productCategory.Edit(command.Name, command.Description,command.Picture,
                command.PictureAlt, command.PictureTitle, command.Keywords,
                command.MetaDescription, slug);

            _ProductCategoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditProductCategory GetDetails(long id)
        {
            return _ProductCategoryRepository.GetDetails(id);
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            return _ProductCategoryRepository.Search(searchModel);
        }
    }
}
