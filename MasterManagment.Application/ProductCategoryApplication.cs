using System.Threading.Tasks;
using _01_FrameWork.Application;
using MasterManagement.Domain.ProductCategoryAgg;
using MasterManagment.Application.Contracts.ProductCategory;
using MasterManagment.Application.Contracts.UnitOfWork;

namespace MasterManagment.Application;

public class ProductCategoryApplication : IProductCategoryApplication
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IMasterUnitOfWork _unitOfWork;

    public ProductCategoryApplication(IProductCategoryRepository ProductCategoryRepository, IMasterUnitOfWork unitOfWork)
    {
        _productCategoryRepository = ProductCategoryRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<OperationResult> CreateAsync(CreateProductCategoryCommand command)
    {
        var operation = new OperationResult();
        if (await _productCategoryRepository.IsExistsAsync(x => x.Name == command.Name))
            return operation.Failed("امکان ثبت رکورد تکراری وجود ندارد .لطفا مجدد تلاش کنید");

        var slug = command.Slug!.Slugify();


        var productCategory = new ProductCategory(command.Name!, command.Description!,
             command.Picture!, command.PictureAlt!, command.PictureTitle!, command.Keywords!,
            command.MetaDescription!, slug);

        await _productCategoryRepository.CreateAsync(productCategory);
        await _unitOfWork.CommitAsync();
        //_productCategoryRepository.SaveChanges();
        await _unitOfWork.CommitAsync();

        return operation.Succedded();
    }

    public async Task<OperationResult> EditAsync(EditProductCategoryCommand command)
    {
        var operation = new OperationResult();
        var productCategory = await _productCategoryRepository.GetAsync(command.Id);

        if (productCategory == null)
            return operation.Failed("رکورد با اطلاعات درخواست شده یافت نشد .لطفا مجدد تلاش بفرمایید");

        if (await _productCategoryRepository.IsExistsAsync(x => x.Name == command.Name && x.Id != command.Id))
            return operation.Failed("امکان ثبت رکورد تکراری وجود ندارد .لطفا مجدد تلاش کنید");

        var slug = command.Slug!.Slugify();



        productCategory.Edit(command.Name!, command.Description!, command.Picture!,
            command.PictureAlt!, command.PictureTitle!, command.Keywords!,
            command.MetaDescription!, slug);
        await _unitOfWork.CommitAsync();
        //_productCategoryRepository.SaveChanges();
        return operation.Succedded();
    }

    public EditProductCategoryCommand GetDetails(long id)
    {
        return _productCategoryRepository.GetDetails(id)!;
    }

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
    {
        return _productCategoryRepository.Search(searchModel);
    }
    public async Task<OperationResult> DeleteAsync(long id)
    {
        var operation = new OperationResult();

        var product = await _productCategoryRepository.GetById(id);
        if (product == null)
            return operation.Failed("دسته بندی یافت نشد");

        //await _productCategoryRepository.DeleteAsync(product);
        product.SoftDelete();
        await _unitOfWork.CommitAsync();

        return operation.Succedded("دسته بندی با موفقیت حذف شد");
    }

    public async Task<List<ProductCategoryViewModel>> GetAll()
    {
        
        return await _productCategoryRepository.GetAllCategories();
      
    }
}
