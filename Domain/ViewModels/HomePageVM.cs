using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class HomePageVM
    {
        public List<OurClient>?   ourClients { get; set; }
        public List<LastestNews>?   LastestNews { get; set; }
        public List<Contact>?    contacts { get; set; }
        public List<Category>?      categories { get; set; }
        public List<Product>?      products { get; set; }
        public Product     getproduct { get; set; }
    

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public Profile Profile { get; set; }
        public List<Profile>? profiles { get; set; }
        public List<Vendor>? vendors { get; set; }

        public List<Block>? Blocks { get; set; }

        //public List<Feed>?   feeds { get; set; }
        //public List<Solution>?     solutions { get; set; }
        public ProductRequest productRequests { get; set; }
        //public List<Promotion>?    promotions { get; set; }

    }
}
