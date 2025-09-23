using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterManagement.Domain.ProductAgg;
using MasterManagement.Domain.ProductCategoryAgg;
using MasterManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Context
{
    public class MasterContext : DbContext
    {
      
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }


        public MasterContext(DbContextOptions<MasterContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(ProductCategoryMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
