using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MasterManagment.Application.Contracts.ProductCategory
{


    public class EditProductCategoryViewModel
    {
        [Required(ErrorMessage = "نام دسته‌بندی الزامی است")]
        [StringLength(100, ErrorMessage = "حداکثر طول نام 100 کاراکتر است")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "توضیحات نمی‌تواند بیش از 500 کاراکتر باشد")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "آدرس تصویر معتبر نیست")]
        public string? Picture { get; set; }

        public string? PictureAlt { get; set; }
        public string? PictureTitle { get; set; }

        [StringLength(250, ErrorMessage = "کلمات کلیدی بیش از 250 کاراکتر نمی‌تواند باشد")]
        public string? Keywords { get; set; }

        [StringLength(500, ErrorMessage = "متا توضیحات بیش از 500 کاراکتر نمی‌تواند باشد")]
        public string? MetaDescription { get; set; }

        public string? Slug { get; set; }
    }

}
