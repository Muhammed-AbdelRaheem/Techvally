using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Enter a valid e-mail address")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string StatusMessage { get; set; }
    }
}
