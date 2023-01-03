using System;
using Domain.Identity.RoleClaims;
using Domain.Identity.Roles;
using Domain.Identity.UserClaim;
using Domain.Identity.UserLogins;
using Domain.Identity.UserRoles;
using Domain.Identity.Users;
using Domain.Identity.UserTokens;
using Domain.PartReviewDetails;
using Domain.PartReviews;
using Domain.Parts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SqlServ4r.EntityFramework
{
    public class DreamContext  : IdentityDbContext<User,Role,Guid,UserClaim,UserRole,UserLogin,RoleClaim,UserToken>
    {
        public DbSet<PartReview> PartReviews { get; set;}
        public DbSet<Part> Parts { get; set;}
        public DbSet<PartReviewDetail> PartReviewDetails { get; set;}

        public DreamContext(DbContextOptions<DreamContext> options):base(options)
        {
            
            
        }
        
        protected override void OnModelCreating (ModelBuilder builder) {

            base.OnModelCreating (builder); 

            //get rid of prefix Asp
            foreach (var entityType in builder.Model.GetEntityTypes ()) {
                var tableName = entityType.GetTableName ();
                if (tableName.StartsWith ("AspNet")) {
                    entityType.SetTableName (tableName.Substring (6));
                }
            }


            
            
            builder.Entity<PartReviewDetail>().HasKey(sc => new { sc.Id });
            builder.Entity<PartReviewDetail>().HasOne<PartReview>(x => x.PartReview)
                .WithMany(x => x.PartReviewDetails)
                .HasForeignKey(x=>x.PartReviewId);
            
            builder.Entity<PartReviewDetail>().HasOne<Part>(x => x.Part)
                .WithMany(x => x.PartReviewDetails)
                .HasForeignKey(x=>x.PartId);


        }
        
        
        
        
        
    }
}