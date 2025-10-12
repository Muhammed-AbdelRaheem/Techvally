using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Required(ErrorMessage = "Required")]
        public string FullName { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Enter a valid e-mail address")]
        public string Email { get; set; }
        public string? Picture { get; set; }

        [Phone]
        [Display(Name = "Mobile")]
        [RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{10}$", ErrorMessage = "MobileNumberRequired")]
        public string? Mobile { get; set; }
        public Gender? Sex { get; set; }
        public DateTime? Birthday { get; set; }

        public string? StatusMessage { get; set; }
    }
}
