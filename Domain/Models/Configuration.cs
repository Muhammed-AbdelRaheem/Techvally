using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Configuration : BaseEntity
    {


        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Arabic Name")]
        public string ArName { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "English Name")]
        public string EnName { get; set; }


        [DisplayName("Arabic Meta Description")]
        [UIHint("MiniEditor")]
        [MaxLength(500, ErrorMessage = "Maximum characters is 500 character")]
        public string? ArMetaDescription { get; set; }

        [DisplayName("English Meta Description")]
        [UIHint("MiniEditor")]
        [MaxLength(500, ErrorMessage = "Maximum characters is 500 character")]
        public string? EnMetaDescription { get; set; }


        [DisplayName("Arabic Footer Brief")]
        [UIHint("Textarea")]
        public string? ArFooterBrief { get; set; }

        [DisplayName("English Footer Brief")]
        [UIHint("Textarea")]
        public string? EnFooterBrief { get; set; }


        [DisplayName("Arabic Keywords")]
        [UIHint("Tag")]

        public string? ArKeywords { get; set; }

        [DisplayName("English Keywords")]
        [UIHint("Tag")]

        public string? EnKeywords { get; set; }



        [DisplayName("Android Link")]

        public string? Android { get; set; }
        [DisplayName("IOS Link")]

        public string? IOS { get; set; }


        [DisplayName("Facebook Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? Facebook { get; set; }


        [DisplayName("Twitter Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? Twitter { get; set; }


        [DisplayName("Youtube Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? Youtube { get; set; }


        [DisplayName("LinkedIn Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? LinkedIn { get; set; }


        [DisplayName("Instagram Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? Instagram { get; set; }

        [DisplayName("Tiktok Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? Tiktok { get; set; }

        [DisplayName("Snapchat Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? Snapchat { get; set; }


        [DisplayName("Default Email Address")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? DefaultEmail { get; set; }


        [DisplayName("Default Email Name")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? DefaultEmailName { get; set; }


        [DisplayName("Default Notification Emails")]
        [MaxLength(500, ErrorMessage = "Maximum characters is 255 character")]
        [UIHint("Tag")]
        public string? DefaultNotificationEmails { get; set; }


        [DisplayName("Social Picture")]
        [UIHint("PicUploader")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? SocialPicture { get; set; }


        [DisplayName("Website URL")]
        public string? WebsiteURL { get; set; }

        [DisplayName("Email Sender")]

        public string? EmailSender { get; set; }

        [DisplayName("Password Email Sender")]
        public string? PasswordEmailSender { get; set; }

        [DisplayName("Port")]
        public int? Port { get; set; }

        [DisplayName("Use SSL")]
        [DefaultValue(false)]
        public bool UseSSL { get; set; }

        [DisplayName("Host")]
        public string? Host { get; set; }


        [DisplayName("Google Analytics Code")]
        [MaxLength(1000, ErrorMessage = "Maximum characters is 1000 character")]
        [UIHint("MiniEditor")]
        public string? GoogleAnalytics { get; set; }

        [DisplayName("Google Analytics Emails")]
        [MaxLength(1000, ErrorMessage = "Maximum characters is 1000 character")]
        public string? GoogleAnalyticsEmail { get; set; }

        [DisplayName("SEO Scripts")]
        [UIHint("Textarea")]
        public string? SEOScripts { get; set; }


        [DisplayName("Enable Subscription")]
        [DefaultValue(false)]
        public bool EnableSubscription { get; set; }

        [DisplayName("Enable OTP")]
        [DefaultValue(false)]
        public bool EnableOTP { get; set; }


        public int Tax { get; set; }

        [DisplayName("Clean Cart After")]
        public int CleanCartAfter { get; set; }

        [DisplayName("Clean Order After")]
        public int CleanOrderAfter { get; set; }

        [DisplayName("Points Per Order")]
        public int PointsPerOrder { get; set; }


        [DisplayName("PayTabs")]
        [DefaultValue(false)]
        public bool PayTabs { get; set; }

        [DisplayName("Apple Pay")]
        [DefaultValue(false)]
        public bool ApplePay { get; set; }

        [DisplayName("Mada")]
        [DefaultValue(false)]
        public bool Mada { get; set; }

        [DisplayName("STC Pay")]
        [DefaultValue(false)]
        public bool STCPay { get; set; }


        [DisplayName("Any Failed Payment Return To Cart")]
        [DefaultValue(false)]
        public bool AnyFailedPaymentReturnToCart { get; set; }

    }

    public class ConfigurationVM
    {
        [Required(ErrorMessage = "Required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [DisplayName("Name")]
        public string Name { get; set; }


        [DisplayName("Meta Description")]
        [MaxLength(500, ErrorMessage = "Maximum characters is 500 character")]
        public string? MetaDescription { get; set; }


        [DisplayName("Keywords")]
        public string? Keywords { get; set; }


        [DisplayName("Facebook Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? Facebook { get; set; }


        [DisplayName("Twitter Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? Twitter { get; set; }


        [DisplayName("Youtube Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? Youtube { get; set; }


        [DisplayName("LinkedIn Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? LinkedIn { get; set; }


        [DisplayName("Instragram Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? Instragram { get; set; }

        [DisplayName("Tiktok Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? Tiktok { get; set; }

        [DisplayName("Snapchat Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? Snapchat { get; set; }

        [DisplayName("WhatsApp Link")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? WhatsApp { get; set; }


        [DisplayName("DefaultEmail Address")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? DefaultEmail { get; set; }


        [DisplayName("Default Email Name")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? DefaultEmailName { get; set; }


        [DisplayName("Default Notification Emails")]
        [MaxLength(500, ErrorMessage = "Maximum characters is 255 character")]
        public string? DefaultNotificationEmails { get; set; }


        [DisplayName("Social Picture")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? SocialPicture { get; set; }


        [DisplayName("Website URL")]
        public string? WebsiteURL { get; set; }

        [DisplayName("Email Sender")]
        [EmailAddress(ErrorMessage = "Enter a valid e-mail address")]
        public string? EmailSender { get; set; }

        [DisplayName("Password Email Sender")]
        public string? PasswordEmailSender { get; set; }

        [DisplayName("Port")]
        public int? Port { get; set; }

        [DisplayName("Use SSL")]
        [DefaultValue(false)]
        public bool UseSSL { get; set; }



        [DisplayName("Host")]
        public string? Host { get; set; }


        [DisplayName("Google Analytics Code")]
        [MaxLength(1000, ErrorMessage = "Maximum characters is 1000 character")]
        public string? GoogleAnalytics { get; set; }

        [DisplayName("Google Analytics Emails")]
        [MaxLength(1000, ErrorMessage = "Maximum characters is 1000 character")]
        public string? GoogleAnalyticsEmail { get; set; }

        [DisplayName("SEO Scripts")]
        public string? SEOScripts { get; set; }


        [DisplayName("PayTabs")]
        [DefaultValue(false)]
        public bool PayTabs { get; set; }

        [DisplayName("Apple Pay")]
        [DefaultValue(false)]
        public bool ApplePay { get; set; }

        [DisplayName("Mada")]
        [DefaultValue(false)]
        public bool Mada { get; set; }

        [DisplayName("STC Pay")]
        [DefaultValue(false)]
        public bool STCPay { get; set; }
        public bool AnyFailedPaymentReturnToCart { get; set; }
    }


    public class ConfigurationLightVM
    {
        public string? FooterBrief { get; set; }
        public string? Twitter { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }

    }

}
