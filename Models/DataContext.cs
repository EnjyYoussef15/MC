
using MCSHiPPERS_Task.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MCSHiPPERS_Task.Models;
public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
       


        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
           
        //    builder.ApplyConfiguration(new RoleConfiguration());

        //}

        //protected virtual void OnModelCreating(ModelBuilder builder) {
        //    base.OnModelCreating(builder);

        //    builder.ApplyConfiguration(new RoleConfiguration());

        //    builder.Entity<Favorite>(eb =>
        //    {
        //        eb.HasNoKey();
        //    });
        //}
    }

