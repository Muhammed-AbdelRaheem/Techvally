using Data.Context;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IUnitofWork
    {
        IEnumerable<T> Query<T>(Func<ReadDBContext, IQueryable<T>> query);

        IOurClientRepository OurClient { get; }
        ILastestNewsRepository LastestNews { get; }
        IContactRepository Contact { get; }
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IConfigurationRepository Configuration { get; }

        IVendorRepository Vendor { get; }
        ILogRepository Log { get; }
        IBlockRepository Block { get; }
        IProfileRepository Profile { get; }
        IProductRequestRepository ProductRequest { get; }
        IMessageFormRepository MessageForm { get; }


        //IPromotionRepository Promotion { get; }

        //IHPCarouselRepositore    HPCarousel { get; }
        //IFeedRepository        Feed { get; }
        //ISolutionRepository      Solution { get; }
        void Save();
    }
}
