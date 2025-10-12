using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class Log : BaseEntity
    {

        [Required(ErrorMessage = "Required")]
        [MaxLength(450, ErrorMessage = "Maximum characters is 255 character")]
        public string ApplicationUserId { get; set; }


        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string IpAddress { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(50, ErrorMessage = "Maximum characters is 50 character")]
        [Display(Name = "Action")]
        public string Action { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(50, ErrorMessage = "Maximum characters is 50 character")]
        [Display(Name = "Table")]
        public string Table { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Details")]
        public string Details { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser User { get; set; }
    }



    public class LogViewModel
    {

        [SearchableInt]
        [Sortable(Default = false)]
        public int Id { get; set; }


        [Display(Name = "Action")]
        [SearchableString(EntityProperty = "Action")]
        [Sortable(EntityProperty = "Action", Default = true)]
        public string Action { get; set; }

        [SearchableString]
        [Sortable]
        [Display(Name = "Table")]
        public string Table { get; set; }

        [SearchableString]
        [Sortable]
        [Display(Name = "Details")]
        public string Details { get; set; }

        [NestedSearchable]
        [NestedSortable]
        public LogUserViewModel User { get; set; }

        [SearchableString]
        [Sortable]
        [Display(Name = "Ip Address")]
        public string IpAddress { get; set; }

        [Display(Name = "Created Date")]
        [SearchableDateTime]
        [Sortable]
        public DateTime CreatedOnUtc { get; set; } = Extantion.AddUtcTime(3);
    }
    public class LogAPIViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Details")]
        public string Details { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedOnUtc { get; set; }
    }

    public class LogUserViewModel
    {
        [SearchableString]
        [Display(Name = "Full Name")]
        [Sortable]
        public string FullName { get; set; }

        [SearchableString]
        [Display(Name = "Email")]
        [Sortable]
        public string Email { get; set; }

    }
}
