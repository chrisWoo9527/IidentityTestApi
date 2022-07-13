using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sql.Data
{
    public class MirDbContext : IdentityDbContext<User, Role, long>
    {
        public MirDbContext(DbContextOptions<MirDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}