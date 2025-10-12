using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Models
{
    public class MessageForm : BaseEntity
    {
        [Required(ErrorMessage = "{0} is Required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Enter a valid e-mail address")]
        public string Email { get; set; }
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Subject { get; set; }
        [Required(ErrorMessage = "{0} is Required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
