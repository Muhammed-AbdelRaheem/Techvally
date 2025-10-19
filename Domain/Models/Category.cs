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
    public class Category : BaseEntity
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Display(Name = "Picture")]
        public string? Image { get; set; }

        [NotMapped]
        public string? hiddenImage { get; set; }

        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }

        public Category? ParentCategory { get; set; }

        [Display(Name = "Sub Category Category")]

        public List<Category>? Subcategories { get; set; }

        public  ICollection<Product>?  Products { get; set; }




    }
    public class CategoryTableData
    {
        [IncludeInReport(Order = 1)]
        public int Id { get; set; }
        [IncludeInReport(Order = 2)]
        public int? ParentCategoryId { get; set; }

        public string? ParentCategoryName { get; set; }

        [SearchableString()]
        [IncludeInReport(Order = 2)]
        [Sortable]
        public string Name { get; set; }


        [SearchableString()]
        [IncludeInReport(Order = 3)]
        public string? Image { get; set; }


        [IncludeInReport(Order = 5)]
        [Sortable]
        public int DisplayOrder { get; set; }

        [IncludeInReport(Order = 6)]
        public bool Hidden { get; set; }


        [IncludeInReport(Order = 7)]
        [SearchableString]
        [Sortable]
        public string? CreatedOnUtc { get; set; }

        [IncludeInReport(Order = 8)]
        [SearchableString]
        [Sortable]
        public string? UpdatedOnUtc { get; set; }
    }







    public class CategoryVM
    {
        [Display(Name = "Id")]
        public int Id { get; set; }


        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = " Picture")]
        [UIHint("PicUploader")]
        public string? Image { get; set; }


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


        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "English Content")]
        [UIHint("Editor")]
        public string Description { get; set; }

    }
}
