using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.ViewModel
{
    //public class HomePageVM
    //{
    //    public HomeBannerLight? Banner { get; set; }
    //    public AboutUsLight? AboutUsLight { get; set; }
    //    public IEnumerable<FAQLight>? FAQS { get; set; }
    //}

    //public class MobileHomePageVM
    //{
    //    public HomeBannerLight? Banner { get; set; }
    //}

    public class IdentityResultViewModel
    {
        public IEnumerable<ErrorRequestViewModel> Errors { get; set; }
        public bool Succeeded { get; set; }
        public int? ItemId { get; set; }
        public string UserId { get; set; }
        public TokenApiViewModel Data { get; set; }
    }
    public enum PipelineEnvironment
    {
        Dev,
        Stg,
        Prod,
        Local
    }
    public class ErrorRequestViewModel
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
    public class ResponseModel
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public List<object>? Data { get; set; }
    }

    public class DeleteResponse
    {
        public bool Result { get; set; }
        public string? Msg { get; set; }
    }

    public enum PhotoType
    {
        Web,
        WebSmall,
        Mobile
    }

    public class SharedB2BAndB2CVM
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string? Image { get; set; }

        public string? SRC { get; set; }

        [JsonIgnore]
        public MemoryStream? FileStreem { get; set; }

        public bool RestrictedContent { get; set; }

        public List<int> Groups { get; set; }

        [DefaultValue(false)]
        public bool Compressed { get; set; }

        public string? CompressedFile { get; set; }
    }
    public class VerifyReceiptRequest
    {

        [JsonPropertyName("receipt-data")]
        public string receipt_data { get; set; }


        [JsonPropertyName("password")]
        public string password { get; set; }


        [JsonPropertyName("exclude-old-transactions")]
        public bool exclude_old_transactions { get; set; }
    }


    public class VerifyReceiptResponse
    {

        [JsonPropertyName("environment")]
        public string environment { get; set; }


        [JsonPropertyName("latest_receipt_info")]
        public List<VerifyReceiptLastReceiptReceiptResponse>? latest_receipt_infos { get; set; }


        [JsonPropertyName("pending_renewal_info")]
        public List<pending_renewal_info>? pending_renewal_info { get; set; }


        [JsonPropertyName("exclude-old-transactions")]
        public bool exclude_old_transactions { get; set; }
    }

    public class VerifyReceiptLastReceiptReceiptResponse
    {


        [JsonPropertyName("product_id")]
        public string product_id { get; set; }
        [JsonPropertyName("transaction_id")]
        public string transaction_id { get; set; }
        [JsonPropertyName("original_transaction_id")]
        public string original_transaction_id { get; set; }

        [JsonPropertyName("expires_date")]
        public string expires_date { get; set; }

        [JsonPropertyName("expires_date_ms")]
        public string expires_date_ms { get; set; }
    }
    public class pending_renewal_info
    {


        [JsonPropertyName("auto_renew_product_id")]
        public string? auto_renew_product_id { get; set; }
        [JsonPropertyName("auto_renew_status")]
        public string? auto_renew_status { get; set; }
        [JsonPropertyName("original_transaction_id")]
        public string? original_transaction_id { get; set; }
    }

    public class UpdateSubscriptionRequest
    {
        public string? subscriptionId { get; set; }
        public string? transactionId { get; set; }
        public string? originalTransactionId { get; set; }
        public string? transactionReceipt { get; set; }
    }


    public enum SortByDate
    {
        All,
        Today,
        Yesterday,
        Last7Days,
        Last30Days,
        ThisMonth,
        LastMonth,
        DateRange,
    }


    public enum ExportType
    {
        AllSaudiRegistrations,
        SaudiApproved,
        CompletePassportDocument,
        CompleteSaudiRegistrationPending,
        CompleteSaudiRegistrationApproved,
        SaudiMedicalPending,
        SaudiMedicalApproved,
        SaudiFirstApproved,
        SaudiBookingPending,
        SaudiBookingApproved,
        AllNonSaudiRegistrations,
        CompleteNonSaudiRegistrationPending,
        CompleteNonSaudiRegistrationApproved,
        NonSaudiApproved,
        NonSaudiMedicalPending,
        NonSaudiMedicalApproved,
        NonSaudiFirstApproved,
        NonSaudiBookingPending,
        NonSaudiBookingApproved,




    }

    public class JahezApiViewModel
    {


        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Enter a valid e-mail address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "FullName Required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

    }
    public sealed record CursorResponse<T>(T Data);

    public class APIResponse
    {
        public bool Result { get; set; }
        public string? Msg { get; set; }
    }

    public class HomeIndexViewModel
    {
        //public List<Master> Masters { get; set; }
        //public List<MasterCategory> MastersCategories { get; set; }
        //public List<Activities> Activities { get; set; }

        // Optionally preload "block" so you don't call repository inside the view
        public Block HomePageBlock { get; set; }
    }

}
