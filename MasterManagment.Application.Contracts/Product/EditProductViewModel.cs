using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Product
{
    using System.ComponentModel.DataAnnotations;

    namespace MasterManagment.Application.Contracts.Product
    {
        public class EditProductViewModel
        {
         

            [Required(ErrorMessage = "نام محصول الزامی است")]
            [StringLength(150, ErrorMessage = "طول نام محصول نمی‌تواند بیش از 150 کاراکتر باشد")]
            public string Name { get; set; }

            [Url(ErrorMessage = "آدرس تصویر معتبر نیست")]
            public string ImagePath { get; set; }

            [Range(0, double.MaxValue, ErrorMessage = "قیمت نمی‌تواند منفی باشد")]
            public decimal Price { get; set; }

            public string Description { get; set; }

            [Range(0, int.MaxValue, ErrorMessage = "موجودی نمی‌تواند منفی باشد")]
            public int Stock { get; set; }

            [Required(ErrorMessage = "دسته‌بندی محصول الزامی است")]
            public long CategoryId { get; set; }

            public bool IsAvailable { get; set; }

            [Range(0, int.MaxValue, ErrorMessage = "تعداد امتیازات نمی‌تواند منفی باشد")]
            public int TotalRatings { get; set; }

            [Range(0, 5, ErrorMessage = "امتیاز میانگین باید بین 0 تا 5 باشد")]
            public double AverageRating { get; set; }
        }
    }

}
