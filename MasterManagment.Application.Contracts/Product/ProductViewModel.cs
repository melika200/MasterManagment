using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Product
{

    public class ProductViewModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? ImagePath { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
        public long CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool IsAvailable { get; set; }
        public int TotalRatings { get; set; }
        public double AverageRating { get; set; }

    }
}
