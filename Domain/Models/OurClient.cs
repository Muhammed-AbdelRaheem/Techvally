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
    public class OurClient : BaseEntity
    {

        [Display(Name = "Picture")]
        public string? Image { get; set; }

        [NotMapped]
        public string? hiddenImage { get; set; }

        [Required]
        public string Url { get; set; }
    }


    public class ClientDataTable
    {
        [IncludeInReport(Order = 1)]
        public int Id { get; set; }

        [IncludeInReport(Order = 2)]
        public int DisplayOrder { get; set; }

        [IncludeInReport(Order = 3)]
        public bool Hidden { get; set; }


        [SearchableString()]
        [IncludeInReport(Order = 4)]
        public string? Image { get; set; }

        [IncludeInReport(Order = 5)]
        [SearchableString]
        public string? CreatedOnUtc { get; set; }

        [IncludeInReport(Order = 6)]
        [SearchableString]
        public string? UpdatedOnUtc { get; set; }
    }







    public class ClientVM
    {
        [Display(Name = "Id")]
        public int Id { get; set; }


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
    }
}
