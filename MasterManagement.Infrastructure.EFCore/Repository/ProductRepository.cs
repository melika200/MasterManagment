using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using _01_FrameWork.Infrastructure;
using MasterManagement.Domain.ProductAgg;
using MasterManagement.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Repository
{
    public class ProductRepository : RepositoryBase<long, Product>, IProductRepository
    {
        private readonly MasterContext _context;

        public ProductRepository(MasterContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product?> GetById(long id)
        {
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAll(Expression<Func<Product, bool>>? where = null)
        {
            IQueryable<Product> query = _context.Products;
            if (where != null)
                query = query.Where(where);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllWithCategory(Expression<Func<Product, bool>>? where = null)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Category);
            if (where != null)
                query = query.Where(where);

            return await query.ToListAsync();
        }

        public async Task Add(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public Task Update(Product product)
        {
            _context.Products.Update(product);
            return Task.CompletedTask;
        }

        public async Task Delete(long id)
        {
            var product = await GetById(id);
            if (product != null)
                _context.Products.Remove(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(long categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .ToListAsync();
        }
    }
}
