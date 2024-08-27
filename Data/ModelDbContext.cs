using CoreApiInNet.Data.Configurations;
using CoreApiInNet.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreApiInNet.Data
{
    public class ModelDbContext:IdentityDbContext<IdentityUser>
    {
        public ModelDbContext(DbContextOptions options):base(options) 
        { 
        
        }
        public DbSet<DbModelUser> UserModel { get; set; }
        public DbSet<DbModelData> DataModel { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
