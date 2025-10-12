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
using System.Xml.Linq;

namespace Domain.Models
{
    public class LastestNews : BaseEntity
    {

        [Required]
        public string Title { get; set; }


        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = " Picture")]
        [UIHint("PicUploader")]
        public string? Image { get; set; }

        public string PublishDate { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "Description")]
        [UIHint("Editor")]
        public string Description { get; set; }

        [Display(Name = "Show In Home Page")]
        [Required(ErrorMessage = "*")]
        [DefaultValue(false)]
        public bool ShowInHomePage { get; set; }


    }
    public class LastestNewsDataTable
    {
        [IncludeInReport(Order = 1)]
        public int Id { get; set; }

        [SearchableString()]
        [IncludeInReport(Order = 2)]
        public string Title { get; set; }


        [SearchableString()]
        [IncludeInReport(Order = 3)]
        public string? Image { get; set; }

        [SearchableString()]
        [IncludeInReport(Order = 4)]
        public string? PublishDate { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "Description")]
        [UIHint("Editor")]
        [IncludeInReport(Order = 5)]

        public string? Description { get; set; }

        [IncludeInReport(Order = 6)]
        public int DisplayOrder { get; set; }

        [IncludeInReport(Order = 7)]
        public bool Hidden { get; set; }
        [IncludeInReport(Order = 8)]

        public bool ShowInHomePage { get; set; }


        [IncludeInReport(Order = 9)]
        [SearchableString]
        public string? CreatedOnUtc { get; set; }

        [IncludeInReport(Order = 10)]
        [SearchableString]
        public string? UpdatedOnUtc { get; set; }



    }

    

}
