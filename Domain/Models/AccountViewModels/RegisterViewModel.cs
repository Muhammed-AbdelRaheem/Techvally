﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "This {0} field is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "Mobile")]
        [RegularExpression(@"^(\+?\d{9,17})$", ErrorMessage = "Invalid Mobile Number")]
        public string Mobile { get; set; }

        public Gender? Gender { get; set; }

        [UIHint("BirthDate")]
        public DateTime? BirthDay { get; set; }
        
        public UserType UserType { get; set; } = UserType.User;

        [Required(ErrorMessage = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public DeviceType DeviceType { get; set; }

        public string? ErrorMessage { get; set; }
        public string? ReturnUrl { get; set; }
        public int LevelId { get; set; }

    }

}
