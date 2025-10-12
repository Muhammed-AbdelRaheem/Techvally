//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;

//namespace Domain.Models
//{
//    public class Feed
//    {

//        [Required(ErrorMessage = "This {0} field is required")]
//        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
//        [Display(Name = "Name")]
//        public string FName { get; set; }

//        [Required(ErrorMessage = "This {0} field is required")]
//        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
//        [Display(Name = "Name")]
//        public string LName { get; set; }

//        [Required(ErrorMessage = "This {0} field is required")]
//        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
//        [Display(Name = "Email")]
//        [EmailAddress(ErrorMessage = "Enter a valid e-mail address")]
//        public string Email { get; set; }


//        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
//        public string? package { get; set; }

//        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
//        public string? BusinessActivity { get; set; }


//        [Display(Name = "Mobile")]
//        [RegularExpression(@"^(\+?\d{5,17})$", ErrorMessage = "MobileNumberRequired")]
//        public string Mobile { get; set; }

//        [Required(ErrorMessage = "This {0} field is required")]
//        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
//        [Display(Name = "Message")]
//        public string Message { get; set; }


//    }
//}
