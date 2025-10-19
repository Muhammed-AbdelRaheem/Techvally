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
    public class Profile:BaseEntity
    {
        [Required]
        public string HPTitle { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [UIHint("PicUploader")]
        [Display(Name = "Home Page Picture")]
        public string? HPImage { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [UIHint("PicUploader")]
        [Display(Name = "Main Picture")]
        public string? DImage { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "Description")]
        [UIHint("Editor")]
        public string Description { get; set; }

        [NotMapped]
        public string? hiddenImage1 { get; set; }
        [NotMapped]
        public string? hiddenImage2 { get; set; }
    }
    public class ProfileDataTable
    {
        [IncludeInReport(Order = 1)]
        public int Id { get; set; }

        [SearchableString()]
        [IncludeInReport(Order = 2)]
        [Sortable]

        public string? HPTitle { get; set; }


        [SearchableString()]
        [IncludeInReport(Order = 3)]
        public string? HPImage { get; set; }

        [SearchableString()]
        [IncludeInReport(Order = 3)]
        public string? DImage { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "Description")]
        [UIHint("Editor")]
        public string Description { get; set; }




        [IncludeInReport(Order = 6)]
        [Sortable]

        public int DisplayOrder { get; set; }

        [IncludeInReport(Order = 7)]
        public bool Hidden { get; set; }


        [IncludeInReport(Order = 8)]
        [SearchableString]
        [Sortable]

        public string? CreatedOnUtc { get; set; }

        [IncludeInReport(Order = 9)]
        [SearchableString]
        [Sortable]

        public string? UpdatedOnUtc { get; set; }



    }
    public class ProfileVM
    {
        [Display(Name = "Id")]
        public int Id { get; set; }


        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Title")]
        public string HPTitle { get; set; }


        [Required(ErrorMessage = "This {0} field is required")]
        [UIHint("PicUploader")]
        [Display(Name = "Home Page Picture")]
        public string? HPImage { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [UIHint("PicUploader")]
        [Display(Name = "Main Picture")]
        public string? DImage { get; set; }


        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "*")]
        [DefaultValue(false)]
        public bool Deleted { get; set; }

        [Display(Name = "Created On Utc")]
        public DateTime CreatedOnUtc { get; set; } = Extantion.AddUtcTime(3);

        [Display(Name = "Updated On Utc")]
        public DateTime UpdatedOnUtc { get; set; } = Extantion.AddUtcTime(3);

        [Display(Name = "Display Order")]
        [DefaultValue(1)]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Hidden")]
        [DefaultValue(false)]
        public bool Hidden { get; set; }


        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "English Content")]
        [UIHint("Editor")]
        public string Description { get; set; }

    }


}
