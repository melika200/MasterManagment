using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.ProductCategory
{
    public class EditProductCategory:CreateProductCategory
    {
        public long Id { get; set; }
    }
}
