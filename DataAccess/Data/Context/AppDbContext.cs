using DataAccess.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{


    public class WriteDbContextFactory : IDesignTimeDbContextFactory<WriteDbContext>
    {
        public WriteDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WriteDbContext>();
            //optionsBuilder.UseNpgsql(Config.Write_DefaultConnection);
            optionsBuilder.UseSqlServer(Config.Write_DefaultConnection);

            return new WriteDbContext(optionsBuilder.Options);
        }
    }
    public class AppDbContext : IdentityDbContext<
    ApplicationUser, ApplicationRole, string,
    IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {


        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<Block> Blocks { get; set; }

        public DbSet<OurClient> OurClients { get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<LastestNews> lastestNews { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Profile> profiles { get; set; }
        public DbSet<Solution> Solution { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<ProductRequest> ProductRequests { get; set; }
        public DbSet<MessageForm> messageForms { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;



            builder.Entity<Product>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Category>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<OurClient>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Contact>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<LastestNews>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Solution>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Vendor>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Block>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Log>().HasQueryFilter(x => !x.Deleted);




            builder.Entity<Profile>().HasData(
             new Profile { Id = 1, HPTitle = " WHO WE ARE", Description = "TECHVALLEY Started its business activities in 2008 as a Focused Distributor of Professional Audio/ Visual solutions and Security systems.\r\n\r\n\r\n\r\nTECHVALLEY’s Sales office in Egypt with ME office in Dubai and Affiliate Group Office in KSA.\r\n\r\n\r\n\r\nTECHVALLEY is a distributor of ATEN, NRGence, Altuscn, VanCryst, PREMIUM LINE, CTS, ASPEN OPTICS, FIBRAIN& SPEAKER CRAFT.\r\n\r\n\r\n\r\nTECHVALLEY's Vision seeks to be the leader in providing the latest End-to-End PRO A/V and Security Systems for our resellers through on-time delivery with comprehensive Sales and Technical Support. \r\n\r\n\r\n\r\nTECHVALLEY's Mission Is such a vendor. It serves its clients as a trusted ally, providing them with the loyalty of a business partner and the economics of an outside vendor. We make sure that our clients have what they need to run their business as well as possible, with mmaximum efficiency & reliability.", DImage = "", HPImage = "", DisplayOrder = 1 }
             );


            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();
            });


            builder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

        }

    }


    public class WriteDbContext : AppDbContext
    {
        public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options)
        {

        }

    }

    public class ReadDBContext : AppDbContext
    {
        public ReadDBContext()
        {

        }
        public ReadDBContext(DbContextOptions<ReadDBContext> options) : base(options)
        {

        }
        [Obsolete("This context is read-only", true)]
        public new int SaveChanges()
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        [Obsolete("This context is read-only", true)]
        public new int SaveChanges(bool acceptAll)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        [Obsolete("This context is read-only", true)]
        public new Task<int> SaveChangesAsync(CancellationToken token = default)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        [Obsolete("This context is read-only", true)]
        public new Task<int> SaveChangesAsync(bool acceptAll, CancellationToken token = default)
        {
            throw new InvalidOperationException("This context is read-only.");
        }


    }
}