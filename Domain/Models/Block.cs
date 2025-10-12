using Fingers10.ExcelExport.Attributes;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Block : BaseEntity
    {


        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Title")]
        public string Title { get; set; }



        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Line")]
        public string? Line { get; set; }


        [Display(Name = " Content")]
        [UIHint("Editor")]
        public string? Content { get; set; }

     


        [Display(Name = "Picture")]
        [UIHint("PicUploader")]
        public string? Picture { get; set; }


        [Display(Name = "Video URL")]
        [UIHint("Video")]
        public string? VideoURL { get; set; }


        [Display(Name = "Address")]
        [UIHint("Tag")]
        public string? Address { get; set; }


        [Display(Name = "Mobile")]
        [UIHint("Tag")]
        public string? Mobile { get; set; }

        [Display(Name = "Email")]
        [UIHint("Tag")]
        public string? Email { get; set; }

        [Display(Name = "Fax")]
        [UIHint("Tag")]
        public string? Fax { get; set; }

        [Display(Name = "Location")]
        [UIHint("Iframe")]
        public string? Location { get; set; }

        public BlockType BlockType { get; set; }
        [Display(Name = "Description")]
        [UIHint("Editor")]
        public string? Description { get; set; }


    }

    public enum BlockType
    {
        [Display(Name = "Contact Us")]
        Contactus = 0,
        [Display(Name = "About Us")]
        AboutUs = 1,
        [Display(Name = "FeedBack")]
        FeedBack = 2,
        [Display(Name = "Forgot Password")]
        ForgotPassword,
        [Display(Name = "LastestNews")]
        LastestNews,
        [Display(Name = "Notify User Subscription")]
        NotifyUserForSubscription,
        [Display(Name = "First Approve")]
        FirstApprovedEmailTemplate,
        [Display(Name = "First Security Approval")]
        FirstSecurityApprovalEmailTemplate,
        [Display(Name = "First Medical Approval")]
        FirstMedicalApprovalEmailTemplate,
        [Display(Name = "Complete Passport Document")]
        CompletePassportDocumentEmailTemplate,
        [Display(Name = "Book Ticket")]
        BookTicket,
        [Display(Name = "Home Page")]
        HomePage,
        [Display(Name = "Sign Up Banner")]
        SignUpBanner,
        [Display(Name = "Thanks Email")]
        ThanksEmail,
        [Display(Name = "Profile")]
        Profile

    }


    public class BlockVM
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string? Content { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string? Picture { get; set; }

        [JsonProperty(PropertyName = "type")]
        public BlockType BlockType { get; set; }

    }


    public class BlockDataTable
    {
        [IncludeInReport(Order = 1)]
        public int Id { get; set; }


        [SearchableString()]
        [IncludeInReport(Order = 2)]
        public string Title { get; set; }



        [SearchableString()]
        [IncludeInReport(Order = 3)]
        public string? Picture { get; set; }


        [IncludeInReport(Order = 5)]
        public int DisplayOrder { get; set; }

        [IncludeInReport(Order = 6)]
        public bool Hidden { get; set; }


        [IncludeInReport(Order = 7)]
        [SearchableString]
        public string? CreatedOnUtc { get; set; }

        [IncludeInReport(Order = 8)]
        [SearchableString]
        public string? UpdatedOnUtc { get; set; }
    }

    #region Web Models





    public class BlockLightVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? name { get; set; }
        public string? content { get; set; }
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Content { get; set; }

    }


    public class FeedbackVM
    {

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Enter a valid e-mail address")]
        public string Email { get; set; }

        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "This {0} field is required")]
        [RegularExpression(@"^(\+?\d{11,17})$", ErrorMessage = "MobileNumberRequired")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Message")]
        public string Message { get; set; }


    }

    #endregion

}