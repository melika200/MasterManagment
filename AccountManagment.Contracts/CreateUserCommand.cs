using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagment.Contracts
{
    public class CreateUserCommand
    {
        public string? Fullname { get; set; }

        [Display(Name = "شماره موبایل", Prompt = "شماره موبایل")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        public string? Username { get; set; }

        [Display(Name = "رمز عبور", Prompt = "رمز عبور")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string? Mobile { get; set; }
        public int RoleId { get; set; }
    }
}
