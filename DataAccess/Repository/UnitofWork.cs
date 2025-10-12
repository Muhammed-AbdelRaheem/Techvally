using AutoMapper;
using Data.Context;
using Dataaccess.Repository;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Project.DataAccess.Repository;
using Project.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UnitofWork : IUnitofWork
    {
        private readonly WriteDbContext _writeDb;
        private readonly ReadDBContext _readDb;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;

        public UnitofWork(WriteDbContext writeDb, ReadDBContext readDb, IMemoryCache memoryCache,IMapper mapper)
        {
            _writeDb = writeDb;
            _readDb = readDb;
            _memoryCache = memoryCache;
            _mapper = mapper;
            Product = new ProductRepository(_writeDb,_readDb,_mapper);
            Category = new CategoryRepository(_writeDb, _readDb, Product,_mapper);
            Contact = new ContactRepository(_writeDb, _readDb, _mapper);
            Configuration = new ConfigurationRepository(_writeDb,_memoryCache);

            OurClient=  new OurClientRepository(_writeDb, _readDb, _mapper);
            Vendor = new VendorRepository(_writeDb, _readDb, _mapper);
            Log = new LogRepository(_writeDb, _mapper, _readDb);
            Block = new BlockRepository(_writeDb, _readDb, _mapper);
            Profile = new ProfileRepository(_writeDb, _readDb, _mapper);
            LastestNews = new LastestNewsRepository(_writeDb, _readDb, _mapper);
            ProductRequest = new ProductRequestRepository(_writeDb);
            MessageForm = new MessageFormRepository(_writeDb);



            //Feature = new FeatureRepository(_db);
            //HPCarousel = new HPCarouselRepositore(db);
            //Feed = new FeedRepository(db);
            //OurClient = new OurClientRepository(db);
            //Promotion = new PromotionRepository(db);
            //Solution = new SolutionRepository(db);
        }
        public IEnumerable<T> Query<T>(Func<ReadDBContext, IQueryable<T>> query)
        {
            return query(_readDb).ToList();
        }


        public IOurClientRepository    OurClient { get; private set; }
        public ILastestNewsRepository     LastestNews { get; private set; }
        public IContactRepository     Contact { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IConfigurationRepository Configuration { get; private set; }

        public IVendorRepository Vendor { get; private set; }
        public ILogRepository Log { get; private set; }
        public IBlockRepository Block { get; private set; }
        public IProfileRepository Profile { get; private set; }
        public IProductRequestRepository ProductRequest { get; private set; }
        public IMessageFormRepository MessageForm { get; private set; }


        //public IPromotionRepository      Promotion { get; private set; }
        //public ISolutionRepository       Solution { get; private set; }

        ////public IFeatureRepository Feature { get; private set; }
        //public IHPCarouselRepositore  HPCarousel { get; private set; }


        //public IFeedRepository      Feed { get; private set; }



        public void Save()
        {
            _writeDb.SaveChanges();
        }

    }
}
