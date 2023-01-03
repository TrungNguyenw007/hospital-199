using Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DBServer.EntityFrameworkCore
{
    public class DreamContext : IdentityDbContext<User>
    {
        public DreamContext(DbContextOptions<DreamContext> options) : base(options)
        {
            DreamContext a;
            a.Users.
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            //get rid of prefix Asp
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }


        }
    }
}