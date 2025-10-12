using Fingers10.ExcelExport.Attributes;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{

    public class ApplicationRole : IdentityRole<string>
    {
        public ApplicationRole() { }

        public ApplicationRole(string roleName)
            : base(roleName)
        {
        }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; } = new List<ApplicationUserRole>();
    }
    public class ApplicationUserRole : IdentityUserRole<string>
    {

        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {


        [Display(Name = "FullName")]
        public string? FullName { get; set; }

        public string? CountryCode { get; set; }


        public string Mobile { get; set; }

        public string? Picture { get; set; }

        public UserType UserType { get; set; }

        public string? RefreshToken { get; set; }


        public string? NationalityId { get; set; }
        public int? RegistrationId { get; set; }

        public DateTime? RefreshTokenExpiryUTC { get; set; }


        public bool Deleted { get; set; }

        public bool Active { get; set; }

        public string? MobileAppId { get; set; }

        public string? Language { get; set; }


        public string? LoginDevice { get; set; }

        public string? LoginIPAddress { get; set; }

        public string? LoginIpCity { get; set; }

        public string? LoginIpCountry { get; set; }

        public string? LoginLocation { get; set; }


        public string? RegisterDevice { get; set; }

        public string? RegisterIPAddress { get; set; }

        public string? RegisterIpCity { get; set; }

        public string? RegisterIpCountry { get; set; }

        public string? RegisterLocation { get; set; }

        [DefaultValue(false)]
        public bool IsMobileDevice { get; set; }

        public string? Headers { get; set; }

        public string? EndpointArn { get; set; }

        public bool HasTopic { get; set; }

        [DefaultValue(false)]
        public bool EnableNotification { get; set; }

        public string? TopicArn { get; set; }

        public DateTime? BirthDay { get; set; }
        public Gender? Gender { get; set; }


        [DefaultValue(false)]
        public bool IsOTPEnabled { get; set; }

        public DeviceType DeviceType { get; set; }

        [DefaultValue(nameof(Provider.Web))]
        public Provider Provider { get; set; }

        public int? LevelId { get; set; }

        [Display(Name = "Is Subscriber")]
        [DefaultValue(false)]
        public bool IsSubscriber { get; set; }

        [UIHint("Date")]
        [Display(Name = "Subscription Till")]
        public DateTime? SubscriptionTill { get; set; }

        public string? AppleClientSecret { get; set; }

        public string? OriginalTransactionId { get; set; }

        public string? DeletionRequestURL { get; set; }
        public string? DeletionConfirmCode { get; set; }
        public string? AppleAccessToken { get; set; }
        public string? AppleRefreshToken { get; set; }
        public string? AppleTokenType { get; set; }
        public string? GoogleAccessToken { get; set; }
        public string? FacebookAccessToken { get; set; }
        public int? Age { get; set; }
        public double? Weight { get; set; }
        public double? TotalScorePoint { get; set; }

        [Display(Name = "Created On Utc")]
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated On Utc")]
        public DateTime UpdatedOnUtc { get; set; } = DateTime.UtcNow;
        public virtual ICollection<ApplicationUserRole>? UserRoles { get; } = new List<ApplicationUserRole>();
        //public ICollection<SNSSubscription>? SNSSubscription { get; set; }
        //public ICollection<UserNotification>? UserNotification { get; set; }
        //public Registration? Registration { get; set; }

    }
    public enum AgeGroup
    {
        [Display(Name = "From 18 to 25")]
        From18To25,
        [Display(Name = "From 26 to 40")]
        From26To40,
        [Display(Name = "From 41 to 55")]
        From41To55,
        [Display(Name = "Above 55")]
        Above55,
    }



    public class UserNotifictionInfo
    {
        public string Email { get; set; }
        public string? MobileAppId { get; set; }
        public DeviceType DeviceType { get; set; }


    }
    public class UserInfoModel
    {
        public string? Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string? Mobile { get; set; }
        public string? Image { get; set; }

        public string? MobileAppId { get; set; }

        public bool Active { get; set; }

    }
    public class UserViewModel
    {

        public string? Id { get; set; }

        [Display(Name = "FullName")]
        [Required(ErrorMessage = "This {0} field is required")]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "The {0} field is not a valid e-mail address.")]
        [Required(ErrorMessage = "This {0} field is required")]
        public string Email { get; set; }



        [RegularExpression(@"\+\d{1,4}", ErrorMessage = "Country code must start with a plus sign followed by 1 to 4 digits.")]
        public string CountryCode { get; set; }

        [Display(Name = "Mobile")]
        [RegularExpression(@"^(\+?\d{9,17})$", ErrorMessage = "Invalid Mobile Number")]
        [Required(ErrorMessage = "This {0} field is required")]
        public string Mobile { get; set; }
        public string? NationalityId { get; set; }
        public int? RegistrationId { get; set; }

        [Display(Name = "Picture")]
        [UIHint("PicUploader")]
        public string? Picture { get; set; }

        [Required]
        public List<string> Role { get; set; }

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

        public UserType UserType { get; set; }

        [DefaultValue(true)]
        [Display(Name = "Enable Notification")]
        public bool EnableNotification { get; set; }


        [Required(ErrorMessage = "This {0} field is required")]
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

        public int? Age { get; set; }
        public double? Weight { get; set; }
        [Display(Name = "Level")]
        public int? LevelId { get; set; }
    }

    public class UserTableViewModel
    {

        [SearchableString]
        [Sortable(Default = false)]
        public string Id { get; set; }

        [IncludeInReport(Order = 1)]
        [SearchableString]
        [Sortable]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [IncludeInReport(Order = 2)]
        [SearchableString]
        [Sortable]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [IncludeInReport(Order = 3)]
        [SearchableString]
        [Sortable]
        [Display(Name = "Mobile")]
        public string? Mobile { get; set; }


        [SearchableEnum(typeof(Gender))]
        [IncludeInReport(Order = 4)]
        [Sortable]
        [Display(Name = "Gender")]
        public string? Gender { get; set; }

        [IncludeInReport(Order = 6)]
        [Searchable]
        [Sortable]
        [Display(Name = "Picture")]
        public string? Picture { get; set; }

        [IncludeInReport(Order = 7)]
        [Searchable]
        [Sortable]
        [Display(Name = "BirthDay")]
        public string? BirthDay { get; set; }

        [IncludeInReport(Order = 8)]
        [Searchable]
        [Sortable]
        [Display(Name = "Active")]
        public bool Active { get; set; }

        [IncludeInReport(Order = 8)]
        [Searchable]
        [Sortable]
        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        [SearchableString]
        [Sortable]
        [Display(Name = "Refresh Token")]
        public string? RefreshToken { get; set; }

        [SearchableString]
        [Sortable]
        [Display(Name = "Refresh Token Expiry UTC")]
        public DateTime? RefreshTokenExpiryUTC { get; set; }


        [Display(Name = "Email Confirmed")]
        [DefaultValue(false)]
        public bool EmailConfirmed { get; set; }


        [Display(Name = "Email Notification")]
        [DefaultValue(false)]
        public bool EnableNotification { get; set; }


        [Searchable]
        [Sortable]
        [Display(Name = "Roles")]
        public string? Roles { get; set; }


        public string? NationalityId { get; set; }
    }

    public class UserTableFrontViewModel
    {

        public string Id { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }


        [Display(Name = "Active")]
        public bool Active { get; set; }
    }


    public class Data
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("application")]
        public string Application { get; set; }

        [JsonProperty("data_access_expires_at")]
        public long DataAccessExpiresAt { get; set; }

        [JsonProperty("expires_at")]
        public long ExpiresAt { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        [JsonProperty("issued_at")]
        public long IssuedAt { get; set; }

        [JsonProperty("scopes")]
        public string[] Scopes { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    public class AppleResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public long ExpireIn { get; set; }


        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

    }

    public enum Gender
    {
        [Display(Name = "Male")]
        Male = 1,
        [Display(Name = "Female")]
        Female
    }

    public class RevokeVM
    {
        public string userId { get; set; }
    }
    public class LoginApiViewModel
    {
        [Required]
        [Display(Name = "Nationality")]
        public string NationalityId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string? Language { get; set; }

        public string? MobileAppId { get; set; }
        public DeviceType DeviceType { get; set; }
    }
    public enum DeviceType
    {
        [Display(Name = "Apple iOS")]
        IOS = 1,
        [Display(Name = "Android")]
        Android,
        [Display(Name = "WEB")]
        WEB,

    }

    public enum Provider
    {
        [Display(Name = "Google")]
        Google = 1,
        [Display(Name = "Apple")]
        Apple,
        [Display(Name = "Facebook")]
        Facebook,
        [Display(Name = "Web")]
        Web,
    }

    public class UserNotifactionVm
    {
        public string? Id { get; set; }

        public string? EndpointArn { get; set; }
        public string? Language { get; set; }

        public DeviceType DeviceType { get; set; }

    }

    public class UserTrainerAssessmentDataTable
    {
        public string? Id { get; set; }

        [SearchableString]
        [IncludeInReport(Order = 2)]
        public string? FullName { get; set; }

        [IncludeInReport(Order = 3)]
        [SearchableString]
        [Display(Name = "Mobile")]
        public string? Mobile { get; set; }

        [IncludeInReport(Order = 3)]
        [SearchableString]
        [Display(Name = "Mobile")]
        public string? Email { get; set; }

        [SearchableString]
        [IncludeInReport(Order = 8)]
        public string? Picture { get; set; }

        [SearchableString]
        [IncludeInReport(Order = 4)]
        public string? BirthDay { get; set; }

        [SearchableString]
        [IncludeInReport(Order = 5)]
        public string? Gender { get; set; }


        [SearchableString]
        [IncludeInReport(Order = 6)]
        [Display(Name = "Level")]
        public string? Level { get; set; }

    }

    public class UserTrainerAssessmentEdit
    {
        public string? Id { get; set; }
        public int TrainerId { get; set; }
        public string? TrainerUserId { get; set; }

        public string? FullName { get; set; }
        public string? Mobile { get; set; }
        public string? BirthDay { get; set; }
        public string? Gender { get; set; }
        public string? Level { get; set; }

        public string? Email { get; set; }


        [Display(Name = "Level")]
        [Required]
        public int LevelId { get; set; }

        [UIHint("Textarea")]
        [Display(Name = "Comment")]
        [Required]
        public string Comment { get; set; }

        [Display(Name = "Created On Utc")]
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

    }

    public class TraineeUpcomingSessionsDataTable
    {

        [SearchableInt]
        public int Id { get; set; }
        public string? Trainer { get; set; }

        [SearchableDateTime]
        public DateTime Date { get; set; }

        [SearchableString]
        [IncludeInReport(Order = 3)]
        public string? Time { get; set; }

    }

    public class TrainerUpcomingSessionsVM
    {

        public int Id { get; set; }

        public int CourtId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? Time { get; set; }

    }
    public class RegisterApiViewModel
    {
        [Required(ErrorMessage = "This {0} field is required")]
        public string FullName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "This {0} field is required")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Country Code is required.")]
        [RegularExpression(@"\+\d{1,4}", ErrorMessage = "Country code must start with a plus sign followed by 1 to 4 digits.")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [RegularExpression(@"^(\+?\d{8,17})$", ErrorMessage = "Invalid Mobile Number")]
        public string Mobile { get; set; }

        public DeviceType DeviceType { get; set; }

        [JsonIgnore]
        public UserType UserType { get; set; } = UserType.User;


        [Required(ErrorMessage = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string? MobileAppId { get; set; }

    }

    public class CompleteRegister
    {
        public string? Id { get; set; }
        public int LevelId { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
    }
    public class ForgetPasswordApi
    {

        [Required]
        public string NationalityId { get; set; }
        public string? Code { get; set; }
        public string? Password { get; set; }
    }
    public class ChangePasswordApi
    {
        [Required]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Required")]
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }


    public enum UserType
    {
        [Display(Name = "Dashboard")]
        Dashboard,
        [Display(Name = "User")]
        User,
        [Display(Name = "Guest")]
        Guest,
        [Display(Name = "Trainer")]
        Trainer,
        [Display(Name = "Admin")]
        Admin,
    }

    public class UserLightVM
    {
        public string? Id { get; set; }

        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Mobile")]
        [RegularExpression(@"^(\+?\d{11,17})$", ErrorMessage = "Invalid Mobile Number")]
        [Required(ErrorMessage = "This {0} field is required")]
        public string Mobile { get; set; }

        public Gender? Gender { get; set; }


    }



}

