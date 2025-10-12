using Fingers10.ExcelExport.Attributes;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
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
    public class Vendor: BaseEntity
{
        [Required]
        public string Title { get; set; }


        [Display(Name = "Picture")]
        public string? Image { get; set; }

        [NotMapped]
        public string? hiddenImage { get; set; }


        [Required]
        public string Description { get; set; }
        [Required]
        public string Url { get; set; }

        [Display(Name = "Show In Home Page")]
        [Required(ErrorMessage = "*")]
        [DefaultValue(false)]
        public bool ShowInHomePage { get; set; }
    }


    public class VendorDataTable
    {
        [IncludeInReport(Order = 1)]
        public int Id { get; set; }


        [SearchableString()]
        [IncludeInReport(Order = 2)]
        public string Title { get; set; }

        [SearchableString()]
        [IncludeInReport(Order = 4)]
        public string? Image { get; set; }


        [IncludeInReport(Order = 2)]
        public int DisplayOrder { get; set; }

        [IncludeInReport(Order = 3)]
        public bool Hidden { get; set; }


        [IncludeInReport(Order = 5)]
        [SearchableString]
        public string? CreatedOnUtc { get; set; }

        [IncludeInReport(Order = 6)]
        [SearchableString]
        public string? UpdatedOnUtc { get; set; }

        [Required]
        public string Url { get; set; }

    }







    public class VendorVM
    {
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Name")]
        public string Title { get; set; }

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

        public string Url { get; set; }
        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = " Picture")]
        [UIHint("PicUploader")]
        public string? Image { get; set; }


        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "Description")]
        [UIHint("Editor")]
        public string Description { get; set; }
    }
}
