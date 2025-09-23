using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.ProductCategoryAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using MasterManagment.Application.Contracts.ProductCategory;

namespace MasterManagement.Infrastructure.EFCore.Repository
{
    public class ProductCategoryRepository : RepositoryBase<long, ProductCategory>, IProductCategoryRepository
    {
        private readonly MasterContext _context;

        public ProductCategoryRepository(MasterContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ProductCategory> GetById(long id)
        {
            return await _context.ProductCategories.FindAsync(id);
        }





        public EditProductCategoryCommand GetDetails(long id)
        {
            return _context.ProductCategories.Select(x => new EditProductCategoryCommand()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug
            }).FirstOrDefault(x => x.Id == id);
        }





        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            var query = _context.ProductCategories.Select(x => new ProductCategoryViewModel
            {
                Id = x.Id,
                Picture = x.Picture,
                Name = x.Name,
                CreationDate = x.CreationDate.ToString()
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
