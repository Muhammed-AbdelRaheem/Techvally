
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;

namespace Models.ViewModel
{
    public class PaginationVenderVM
    {

        public List<Vendor> vendors { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }


    }
}
