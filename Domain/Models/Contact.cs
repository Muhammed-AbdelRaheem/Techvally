using Fingers10.ExcelExport.Attributes;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Contact : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public string? Tel { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }

    }

    public class ContactDataTable
    {
        [IncludeInReport(Order = 1)]
        public int Id { get; set; }


        [SearchableString()]
        [IncludeInReport(Order = 2)]
        public string Title { get; set; }


        [SearchableString()]
        [IncludeInReport(Order = 3)]
        public string? Address { get; set; }
        [SearchableString()]
        [IncludeInReport(Order = 4)]
        public string? Tel { get; set; }
        [SearchableString()]
        [IncludeInReport(Order = 5)]
        public string? Fax { get; set; }



        [IncludeInReport(Order = 6)]
        public int DisplayOrder { get; set; }

        [IncludeInReport(Order = 7)]
        public bool Hidden { get; set; }


        [IncludeInReport(Order = 8)]
        [SearchableString]
        public string? CreatedOnUtc { get; set; }

        [IncludeInReport(Order = 9)]
        [SearchableString]
        public string? UpdatedOnUtc { get; set; }



    }
    public class ContactVm
    {

        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string? Tel { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }

        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "*")]
        [DefaultValue(false)]
        public bool Deleted { get; set; }

        [Display(Name = "Created On Utc")]
        public DateTime CreatedOnUtc { get; set; } = Extantion.AddUtcTime(6);

        [Display(Name = "Updated On Utc")]
        public DateTime UpdatedOnUtc { get; set; } = Extantion.AddUtcTime(6);

        [Display(Name = "Display Order")]
        [DefaultValue(1)]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Hidden")]
        [DefaultValue(false)]
        public bool Hidden { get; set; }
    }

}
