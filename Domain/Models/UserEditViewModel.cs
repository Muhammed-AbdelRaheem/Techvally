using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Display(Name = "FullName")]
        [Required(ErrorMessage = "This {0} field is required")]
        public string FullName { get; set; }


        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "The {0} field is not a valid e-mail address.")]
        [Required(ErrorMessage = "This {0} field is required")]
        //[Remote]
        public string Email { get; set; }

        [Required(ErrorMessage = "Country Code is required.")]
        [RegularExpression(@"\+\d{1,4}", ErrorMessage = "Country code must start with a plus sign followed by 1 to 4 digits.")]
        public string CountryCode { get; set; }

        [Display(Name = "Mobile")]
        [RegularExpression(@"^(\+?\d{9,17})$", ErrorMessage = "Invalid Mobile Number")]
        [Required(ErrorMessage = "This {0} field is required")]
        public string Mobile { get; set; }

        [UIHint("PicUploader")]
        [JsonIgnore]
        public string? Picture { get; set; }

        [JsonPropertyName("picture")]
        public string? MobilePicture { get; set; }

        [UIHint("PicUploader")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Birth Day")]
        public DateTime? BirthDay { get; set; }
        public Gender? Gender { get; set; }


        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [JsonIgnore]
        public string? OldPassword { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [JsonIgnore]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        [JsonIgnore]
        public string? ConfirmNewPassword { get; set; }

        [JsonIgnore]

        public string? Language { get; set; }

        [JsonIgnore]
        [DefaultValue(false)]
        public bool FromAdmin { get; set; }
        [JsonIgnore]
        public UserType UserType { get; set; } = UserType.User;
        public int Age { get; set; }
        public double Weight { get; set; }

        [Display(Name = "Level")]
        public int? LevelId { get; set; }
        public int? BranchId { get; set; }

    }
    public class DashboardEditViewModel
    {
        public string Id { get; set; }

        [Display(Name = "FullName")]
        [Required(ErrorMessage = "This {0} field is required")]
        public string FullName { get; set; }


        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "The {0} field is not a valid e-mail address.")]
        [Required(ErrorMessage = "This {0} field is required")]
        //[Remote]
        public string Email { get; set; }

        [RegularExpression(@"\+\d{1,4}", ErrorMessage = "Country code must start with a plus sign followed by 1 to 4 digits.")]
        public string CountryCode { get; set; }

        [Display(Name = "Mobile")]
        public string? Mobile { get; set; }

        [Required]
        public List<string> Roles { get; set; }



        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmNewPassword { get; set; }


        [DefaultValue(false)]
        public bool? FromAdmin { get; set; }

        public UserType UserType { get; set; } = UserType.Dashboard;






        public string? NationalityId { get; set; }
        public int? RegistrationId { get; set; }

        [Display(Name = "Picture")]
        [UIHint("PicUploader")]
        public string? Picture { get; set; }


        [Display(Name = "Restriction by group")]
        public List<int>? Groups { get; set; }


        [Display(Name = "Birth Day")]
        [UIHint("BirthDate")]
        public DateTime? BirthDay { get; set; }

        [Display(Name = "Gender")]
        public Gender? Gender { get; set; }

        [Display(Name = "Deleted")]
        [DefaultValue(false)]
        public bool Deleted { get; set; }

        [Display(Name = "Active")]
        [DefaultValue(false)]
        public bool Active { get; set; }

        [Display(Name = "Mobile App Id")]
        public string? MobileAppId { get; set; }


        public DeviceType DeviceType { get; set; }

        public int Age { get; set; }
        public double Weight { get; set; }
        [Display(Name = "Level")]
        public int? LevelId { get; set; }






    }

    public class UserEditInfoViewModel
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }

        public string? MobileAppId { get; set; }
        public Gender? Gender { get; set; }

        public AgeGroup? AgeGroup { get; set; }

        [DefaultValue(true)]
        public bool EnableNotification { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string? Language { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int? ProjectId { get; set; }


    }

    public class UserEditInfo
    {
        public string? Id { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "This {0} field is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "This {0} field is required")]
        [EmailAddress]
        public string Email { get; set; }


        [Display(Name = "Mobile")]
        [RegularExpression(@"^(\+?\d{11,17})$", ErrorMessage = "Invalid Mobile Number")]
        [Required(ErrorMessage = "This {0} field is required")]
        public string Mobile { get; set; }

        public AgeGroup? AgeGroup { get; set; }
        public Gender? Gender { get; set; }


        public string? MobileAppId { get; set; }


        [DefaultValue(true)]
        public bool EnableNotification { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string? Language { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int? ProjectId { get; set; }

        [JsonPropertyName("ticketMXAccessToken")]
        public string? AccessTokenTicketMX { get; set; }
    }

    public class UserScoreBoard
    {
        public string? Id { get; set; }

        public string Name { get; set; }
        public string Picture { get; set; }
        public double Points { get; set; }

    }

}
